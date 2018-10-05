using Shared.Invoicing;

namespace Shared.Printing
{
    public class InvoicePrintRequest
    {
        public Invoice InvoiceToPrint { get; set; }
    }

    public class InvoiceNotificationRequest
    {
        public Invoice InvoiceForNotification { get; set; }
    }
}
