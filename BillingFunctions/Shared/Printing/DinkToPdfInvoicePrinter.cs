using DinkToPdf;
using Shared.Invoicing;
using System;
using System.Text;

namespace Shared.Printing
{
    public class DinkToPdfInvoicePrinter : IInvoicePrinter
    {
        private const string HeaderTemplate =
            @"<h1>Invoice {0}</h1>
            <br/>
            <h3>Customer: {1}</h3>
            <h3>Address: 00-101 Warszawa, Chłodna 21</h3>
            <h3>Description: {2}</h3>
            <br/>
            <br/>
            <h3>Details:</h3>
            <table width=""50%"" border=""1"" bgcolor=""#C0C0C0"">
            <tr><th>Item</th><th>Price</th></tr>";

        private const string ItemTemplate = @"<tr><td>{0}</td><td align=""right"">{1}</td></tr>";

        private const string FooterTemplate =
            @"<tr><td><b>Total</b></td><td align=""right""><b>{0}</b></td></tr>
            </table>";

        public byte[] Print(Invoice invoice)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = BuildContent(invoice),
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };
            var converter = new SynchronizedConverter(new PdfTools());
            var pdf = converter.Convert(doc);
            return pdf;
        }

        private string BuildContent(Invoice invoice)
        {
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(BuildHeader(invoice));
            foreach (var item in invoice.Lines)
            {
                contentBuilder.AppendLine(BuildLine(item));
            }
            contentBuilder.AppendLine(BuildFooter(invoice));
            return contentBuilder.ToString();
        }

        private string BuildFooter(Invoice invoice)
        {
            return string.Format(FooterTemplate, invoice.TotalCost);
        }

        private string BuildLine(InvoiceLine item)
        {
            return string.Format(ItemTemplate, item.ItemName, item.Cost.ToString());
            throw new NotImplementedException();
        }

        private string BuildHeader(Invoice invoice)
        {
            return string.Format(HeaderTemplate, invoice.InvoiceNumber, invoice.Customer, invoice.Description);
        }
    }
}
