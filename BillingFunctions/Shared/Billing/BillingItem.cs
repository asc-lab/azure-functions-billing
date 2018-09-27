using Microsoft.WindowsAzure.Storage.Table;

namespace BillingFunctions
{
    public class BillingItem : TableEntity
    {
        public string Beneficiary { get; set; }
        public string ProductCode { get; set; }
        public double Amount { get; set; }
    }
}
