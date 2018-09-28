using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shared.Billing;
using Shared.Invoicing;
using System.IO;

namespace BillingFunctions
{
    public static class GenerateBillingItemsFunc
    {
        [FunctionName("GenerateBillingItemsFunc")]
        public static void Run(
            [BlobTrigger("active-lists/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name,
            [Table("billingItems")] ICollector<BillingItem> billingItems,
            [Queue("invoice-generation-request")] out InvoiceGenerationRequest queueRequest,
            ILogger log)
        {
            log.LogInformation($"C# Blob Trigger function Processed blob: {name} Bytes");

            var activeList = ActiveListParser.Parse(name, myBlob);
            var generator = new BillingItemGenerator();
            foreach (var bi in generator.Generate(activeList))
            {
                billingItems.Add(bi);
            }

            queueRequest = InvoiceGenerationRequest.ForActiveList(activeList);
        }
    }

    
}
