using Shared.Invoicing;

namespace Shared.Printing
{
    public class InvoicePrintRequest
    {
        public Invoice InvoiceToPrint { get; set; }
    }

    public class InvoiceSmsRequest
    {
        public Invoice InvoiceToSms { get; set; }
    }
}
