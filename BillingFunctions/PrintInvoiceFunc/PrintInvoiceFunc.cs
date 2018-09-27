using Microsoft.Azure.WebJobs;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BillingFunctions
{
    public static class PrintInvoiceFunc
    {
        [FunctionName("PrintInvoiceFunc")]
        public static void Run(
            [QueueTrigger("invoice-print-request")]InvoicePrintRequest printRequest,
            Binder binder,
            ILogger log)
        {
            var pdf = new InvoicePrinter().Print(printRequest.InvoiceToPrint);

            StoreResultInBlobAsync(
                binder,
                $"Invoice_{printRequest.InvoiceToPrint.InvoiceNumber.Replace("/","_")}",
                pdf);
        }

        private static async Task StoreResultInBlobAsync(Binder binder, string title, byte[] doc)
        {
            using (var stream = await binder.BindAsync<Stream>(new BlobAttribute($"printouts/{title}.pdf", FileAccess.Write)))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(doc);
                }
            }
        }
    }
}
