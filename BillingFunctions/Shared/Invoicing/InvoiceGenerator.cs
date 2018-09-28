using Shared.Billing;
using System.Collections.Generic;

namespace Shared.Invoicing
{
    public class InvoiceGenerator
    {
        public Invoice Generate(
            InvoiceGenerationRequest request,
            List<BillingItem> billingItems)
        {
            var i = Invoice.Create(request.CustomerCode, request.Year, request.Month);
            i.BillItems(billingItems);
            return i;
        }
    }
}
