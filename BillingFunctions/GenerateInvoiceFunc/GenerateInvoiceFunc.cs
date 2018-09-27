using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BillingFunctions
{
    public static class GenerateInvoiceFunc
    {
        [FunctionName("GenerateInvoiceFunc")]
        public static void Run(
            [QueueTrigger("invoice-generation-request")]InvoiceGenerationRequest request,
            [Table("billingItems")] IQueryable<BillingItem> billingItems,
            [CosmosDB("crm","invoices",ConnectionStringSetting = "cosmosDb")] out dynamic generatedInvoice,
            [Queue("invoice-print-request")] out InvoicePrintRequest printRequest,
            [Queue("invoice-sms-request")] out InvoiceSmsRequest smsRequest,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {request.CustomerCode} {request.Year} {request.Month}");

            var generator = new InvoiceGenerator();
            var invoice = generator.Generate(request, billingItems);

            //output to cosmos db
            generatedInvoice = invoice;
            //output to queue
            printRequest = new InvoicePrintRequest { InvoiceToPrint = invoice };
            smsRequest = new InvoiceSmsRequest { InvoiceToSms = invoice };
        }
    }
}
