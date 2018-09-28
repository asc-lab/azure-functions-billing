using Shared.Billing;

namespace Shared.Invoicing
{
    public class InvoiceGenerationRequest
    {
        public string CustomerCode { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public static InvoiceGenerationRequest ForActiveList(ActiveList activeList)
        {
            return new InvoiceGenerationRequest
            {
                CustomerCode = activeList.CustomerCode,
                Year = activeList.Year,
                Month = activeList.Month
            };
        }
    }
}
