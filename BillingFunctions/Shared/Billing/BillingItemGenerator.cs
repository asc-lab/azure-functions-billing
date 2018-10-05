using Shared.Pricing;
using System;
using System.Collections.Generic;

namespace Shared.Billing
{
    public class BillingItemGenerator
    {
        public List<BillingItem> Generate(ActiveList activeList, PriceList priceList)
        {
            var billingDate = new DateTime(activeList.Year, activeList.Month, 1);
            var billingItems = new List<BillingItem>();

            foreach (var line in activeList.DataLines)
            {
                if (String.IsNullOrWhiteSpace(line))
                    continue;
                
                var benex = Beneficiary.FromCsvLine(line);
                var price = priceList.GetPrice(benex, billingDate);

                billingItems.Add(new BillingItem
                {
                    PartitionKey = $"{activeList.CustomerCode}-{activeList.Year}-{activeList.Month}",
                    RowKey = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.Now,
                    Beneficiary = $"{benex.NationalId} {benex.Name}",
                    ProductCode = benex.ProductCode,
                    Amount = Convert.ToDouble(price)
                });
            }

            return billingItems;
        }
    }
}
