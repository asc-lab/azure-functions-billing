using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Logging;

namespace BillingFunctions
{
    public static class GenerateBillingItemsFunc
    {
        [FunctionName("GenerateBillingItemsFunc")]
        public static void Run(
            [BlobTrigger("active-lists/{name}", Connection = "AzureWebJobsStorage")]CloudBlockBlob myBlob,
            [Table("billingItems")] ICollector<BillingItem> billingItems,
            [Queue("invoice-generation-request")] out InvoiceGenerationRequest queueRequest,
            ILogger log)
        {
            var activeList = ActiveListParser.Parse(myBlob);
            var generator = new BillingItemGenerator();
            foreach (var bi in generator.Generate(activeList))
            {
                billingItems.Add(bi);
            }

            queueRequest = InvoiceGenerationRequest.ForActiveList(activeList);
        }
    }

    
}
