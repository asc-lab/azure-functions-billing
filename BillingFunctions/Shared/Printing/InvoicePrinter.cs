using Shared.Invoicing;

namespace Shared.Printing
{
    interface IInvoicePrinter
    {
        byte[] Print(Invoice invoice);
    }
}
