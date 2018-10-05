using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Shared.Billing;
using Shared.Invoicing;
using Shared.Printing;
using System.Collections.Generic;

namespace BillingFunctions
{
    public static class GenerateInvoiceFunc
    {
        [FunctionName("GenerateInvoiceFunc")]
        public static void Run(
            [QueueTrigger("invoice-generation-request")]InvoiceGenerationRequest request,
            [Table("billingItems")] CloudTable billingItems,
            [CosmosDB("crm", "invoices", ConnectionStringSetting = "cosmosDb")] out dynamic generatedInvoice,
            [Queue("invoice-print-request")] out InvoicePrintRequest printRequest,
            [Queue("invoice-notification-request")] out InvoiceNotificationRequest notificationRequest,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {request.CustomerCode} {request.Year} {request.Month}");

            var generator = new InvoiceGenerator();
            var items = GetBillingItemsFromTable(billingItems, request);
            var invoice = generator.Generate(request, items);

            generatedInvoice = invoice;
            
            printRequest = new InvoicePrintRequest { InvoiceToPrint = invoice };
            notificationRequest = new InvoiceNotificationRequest { InvoiceForNotification = invoice };
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
