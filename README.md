# Azure Functions v2 with .NET Core

Serverless in Insurance world.

<p align="center">
    <img alt="Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-azure-functions/master/readme-images/azure-functions-architecture.png" />
</p>

## Prerequisites

1. Blob Container ```active-lists```.
2. Upload file ```ASC_2018_02_activeLists.txt``` to ```active-lists``` blob.
3. Install [CosmosDB](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).
4. Create in CosmosDB database ```crm``` and collection ```prices``` and ```invoices```.
5. Run project ```PriceDbInitializator``` to init table with prices.

## Run locally

1. Run [Microsoft Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).

## PDF Generation

### JsReport

```bash
docker run -p 80:5488 jsreport/jsreport
```

### DinkToPdf

Instruction [here](https://odetocode.com/blogs/scott/archive/2018/02/14/pdf-generation-in-azure-functions-v2.aspx).