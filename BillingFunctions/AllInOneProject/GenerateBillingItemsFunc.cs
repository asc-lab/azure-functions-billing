using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shared.Billing;
using Shared.Invoicing;
using Shared.Pricing;
using System;
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
            log.LogInformation($"[GenerateBillingItemsFunc] Blob Trigger. Function processed blob: {name} Bytes");

            var activeList = ActiveListParser.Parse(name, myBlob);
            var generator = new BillingItemGenerator();
            var priceList = GetPriceList(activeList.CustomerCode);
            foreach (var bi in generator.Generate(activeList, priceList))
            {
                billingItems.Add(bi);
            }

            queueRequest = InvoiceGenerationRequest.ForActiveList(activeList);
        }

        private static PriceList GetPriceList(string customerCode)
        {
            var url = Environment.GetEnvironmentVariable("PriceDbUrl");
            var auth = Environment.GetEnvironmentVariable("PriceDbAuthKey");
            return PriceRepository.Connect(url, auth).GetPriceList(customerCode);
        }
    }
}
