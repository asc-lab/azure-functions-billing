using System.Net.Http;
using Shared.Invoicing;
using Shared.Printing.JsReport;
using System.Runtime.Serialization.Json;

namespace Shared.Printing
{
    public class JsReportInvoicePrinter : IInvoicePrinter
    {
        private static readonly HttpClient client = new HttpClient();

        public byte[] Print(Invoice invoice)
        {
            var request = new JsReportRequest(
                new Template("INVOICE"),
                new TemplateOptions(),
                invoice
            );

            var pdf = Generate(request);
            return pdf;
        }

        private byte[] Generate(JsReportRequest request)
        {
            var stringTask = client.GetStringAsync("http://localhost:5488/api/report");
            var serializer = new DataContractJsonSerializer(typeof(byte[]));
            //var msg = serializer.ReadObject(await stringTask) as byte[];
            return null;
        }
    }
}
