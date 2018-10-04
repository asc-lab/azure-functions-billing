using System.Net.Http;
using Shared.Invoicing;
using Shared.Printing.JsReport;
using System.Runtime.Serialization.Json;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Shared.Printing
{
    public class JsReportInvoicePrinter : IInvoicePrinter
    {
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
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5488");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = new JsonContent(request);
                
                var response = client.PostAsync("/api/report", json).Result;
                var bytes = response.Content.ReadAsByteArrayAsync().Result;

                return bytes;
            }
        }
    }

    class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }), Encoding.UTF8, "application/json")
        { }
    }
}
