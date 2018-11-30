# Azure Functions v2 with .NET Core

This example shows simplified billing system in serverless architecture.

<p align="center">
    <img alt="Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/azure-functions-architecture.png" />
</p>

1. User uploads CSV file (with name structure ```CLIENTCODE_YEAR_MONTH_activeList.txt.```) with Beneficiaries (the sample file is located in the ```data-examples``` folder) to a specific data storage - ```active-lists``` Azure Blob Container.

2. The above action triggers a function (```GenerateBillingItemsFunc```) that is responsible for:
    * generating billing items (using prices from an external database - CosmosDB ```crm``` database, ```prices``` collection) and saving them in the table ```billingItems```;
    * sending message about the need to create a new invoice to ```invoice-generation-request```;

3. When a new message appears on the queue ```invoice-generation-request```, next function is triggered (```GenerateInvoiceFunc```). This function creates domain object ```Invoice``` and save this object in database (CosmosDB ```crm``` database, ```invoices``` collection) and send message to queues: ```invoice-print-request``` and ```invoice-notification-request```.

4. When a new message appears on the queue ```invoice-print-request```, function ```PrintInvoiceFunc``` is triggered. This function uses external engine to PDF generation - JsReport and saves PDF file in BLOB storage.

5. When a new message appears on the queue ```invoice-notification-request```, function ```NotifyInvoiceFunc``` is triggered. This function uses two external systems - SendGrid to Email sending and Twilio to SMS sending.

## Tutorial from scratch to run locally

1. Install and run [Microsoft Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).

2. Install and run [CosmosDB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator). Check this on ```https://localhost:8081/_explorer/index.html```.

3. Create in Emulator blob Container ```active-lists```.

4. Upload  ```ASC_2018_02_activeLists.txt``` file from ```data-examples``` folder to ```active-lists``` blob.

5. Create CosmosDB database ```crm``` and in this database create collections: ```prices```,  ```invoices```.

6. Add CosmosDB properties ```PriceDbUrl``` and ```PriceDbAuthKey``` to ```local.appsettings.json``` in ```PriceDbInitializator``` and ```GenerateBillingIemsFunc```. You can copy this properties from ```Azure CosmosDB Emulator``` - check point 2 (URI and Primary Key).

7. Run project ```PriceDbInitializator``` to init collection ```prices``` in ```crm``` database.

8. Add CosmosDB connection string as ```cosmosDb``` to ```local.settings.json``` in ```GenerateInvoiceFunc```. You can copy this string from ```Azure CosmosDB Emulator``` - check point 2 (Primary Connection String).

9. Create an account in [SendGrid](https://sendgrid.com/) and add property ```SendGridApiKey``` to ```local.settings.json``` in ```NotifyInvoiceFunc```.

10. Create an account in [Twilio](https://www.twilio.com/) and add properties ```TwilioAccountSid``` ```TwilioAuthToken``` to ```local.settings.json``` in ```NotifyInvoiceFunc```.

11. Run JsReport with Docker: ```docker run -p 5488:5488 jsreport/jsreport```. Check JsReport Studio on ```localhost:5488```.

12. Add JsReport url as ```JsReportUrl``` to ```local.settings.json``` in ```PrintInvoiceFunc``` project.

13. Add JsReport template with name ```INVOICE``` and content:

```html
<h1>Invoice {{invoiceNumber }}</h1>
<h3>Customer: {{ customer }}</h3>
<h3>Address: 00-101 Warszawa, Chłodna 21</h3>
<h3>Description: {{ description }}</h3>

<h3>Details:</h3>
<table width="90%" border="1" bgcolor="#C0C0C0" align="center">
    <tr>
        <th>Item</th>
        <th>Price</th>
    </tr>

    {{#each lines}}
    <tr>
        <td>{{ itemName }}</td>
        <td align="right">{{ cost }}</td>
    </tr>
    {{/each}}

    <tr>
        <td>
            <strong>Total</strong>
        </td>
        <td align="right">
            <strong>{{ totalCost }}</strong>
        </td>
    </tr>
</table>
```

Example JSON for INVOICE template:

```json
{
  "customer": "ASC",
  "invoiceNumber": "ASC/10/2018",
  "description": "Invoice for insurance policies for 10/2018",
  "lines": [
    {
      "itemName": "Policy A",
      "cost": 2140.0
    },
    {
      "itemName": "Policy B",
      "cost": 1360.0
    }
  ],
  "totalCost": 3500.0
}
```

All properties in one `local.appsettings.json`:

```
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "PriceDbUrl": "https://localhost:8081",
    "PriceDbAuthKey": "AUTH_KEY",
    "cosmosDb": "AccountEndpoint=https://localhost:8081/;AccountKey=AUTH_KEY",
    "JsReportUrl": "http://localhost:5488",
    "SendGridApiKey": "SEND_GRID_API_KEY",
    "TwilioAccountSid ": "TWILIO_ACCOUNT_SID",
    "TwilioAuthToken": "TWILIO_AUTH_TOKEN"
  }
}
```

## Monitoring examples

Application Map for all function in one project:

<p align="center">
    <img alt="Application Map 1" 
    src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/application_map_one_project.png" />
</p>

Application Map for functions in separated projects:

<p align="center">
    <img alt="Application Map 2"
    src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/application_map_separated_projects.png" />
</p>

End-to-end transaction details:

<p align="center">
    <img alt="End to End Transaction Details"
    src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/performance.png" />
</p>

## Tips & Tricks

1. CSV file is working for client code ```ASC``` (filename: ```ASC_2018_12_activeList.txt```). If you want run functions for another client code, you must simulate prices in database. Check project ```PriceDbInitializator```, file ```Program.cs```, method ```AddDoc```.

2. Remember that you must use **Twilio Test Credentials**.

<p align="center">
    <img alt="Twilio Test Credentials" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/twilio_test_credentials.png" />
</p>

3. **Microsoft Azure Storage Emulator** with all created storages:

<p align="center">
    <img alt="Microsoft Azure Storage Emulator" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/azure_storage_emulator.png" />
</p>
