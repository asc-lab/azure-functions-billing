using System.Linq;

namespace BillingFunctions
{
    public class InvoiceGenerator
    {
        public Invoice Generate(
            InvoiceGenerationRequest request,
            IQueryable<BillingItem> billingItems)
        {
            var i = Invoice.Create(request.CustomerCode, request.Year, request.Month);
            i.BillItems(billingItems
                .Where(bi => bi.PartitionKey == $"{request.CustomerCode}-{request.Year}-{request.Month}")
                .ToList());
            return i;
        }
    }
}
