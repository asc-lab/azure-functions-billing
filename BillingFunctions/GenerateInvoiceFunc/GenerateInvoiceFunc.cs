using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BillingFunctions
{
    public static class GenerateInvoiceFunc
    {
        [FunctionName("GenerateInvoiceFunc")]
        public static void Run(
            [QueueTrigger("invoice-generation-request")]InvoiceGenerationRequest request,
            [Table("billingItems")] CloudTable billingItems,
            [CosmosDB("crm","invoices", ConnectionStringSetting = "cosmosDb")] out dynamic generatedInvoice,
            [Queue("invoice-print-request")] out InvoicePrintRequest printRequest,
            [Queue("invoice-sms-request")] out InvoiceSmsRequest smsRequest,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {request.CustomerCode} {request.Year} {request.Month}");

            var generator = new InvoiceGenerator();
            var items = GetBillingItemsFromTable(billingItems, request);
            var invoice = generator.Generate(request, items);

            //output to cosmos db
            generatedInvoice = invoice;
            
            //output to queue
            printRequest = new InvoicePrintRequest { InvoiceToPrint = invoice };
            smsRequest = new InvoiceSmsRequest { InvoiceToSms = invoice };
        }

        static List<BillingItem> GetBillingItemsFromTable(CloudTable billingItems, InvoiceGenerationRequest request)
        {
            TableQuery<BillingItem> query = new TableQuery<BillingItem>()
                .Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, $"{request.CustomerCode}-{request.Year}-{request.Month}")
                );

            var querySegment = billingItems.ExecuteQuerySegmentedAsync(query, null);
            var items = new List<BillingItem>();
            foreach (BillingItem item in querySegment.Result)
            {
                items.Add(item);
            }
            return items;
        }
    }
}
