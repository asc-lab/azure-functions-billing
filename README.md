# Azure Functions v2 with .NET Core

Serverless in Insurance world.

<p align="center">
    <img alt="Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/azure-functions-architecture.png" />
</p>

## Prerequisites

1. Install and run [Microsoft Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).
2. Install [CosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).
3. Create blob Container ```active-lists```.
4. Upload  ```ASC_2018_02_activeLists.txt``` file from ```data-examples``` folder to ```active-lists``` blob.
5. Create CosmosDB database ```crm``` and collections: ```prices```,  ```invoices```.
6. Run project ```PriceDbInitializator``` to init table with prices.

## PDF Generation

### JsReport

```bash
docker run -p 5488:5488 jsreport/jsreport
```

### DinkToPdf

Instruction [here](https://odetocode.com/blogs/scott/archive/2018/02/14/pdf-generation-in-azure-functions-v2.aspx).