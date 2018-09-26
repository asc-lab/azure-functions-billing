using System;
using System.Collections.Generic;

namespace BillingFunctions
{
    public class BillingItemGenerator
    {
        public List<BillingItem> Generate(ActiveList activeList)
        {
            var billingDate = new DateTime(activeList.Year, activeList.Month, 1);
            var billingItems = new List<BillingItem>();
            var priceList = PriceRepository.Connect().GetPriceList(activeList.CustomerCode);

            foreach (var line in activeList.DataLines)
            {
                if (String.IsNullOrWhiteSpace(line))
                    continue;
                //Beneficiary from line
                var benex = Beneficiary.FromCsvLine(line);
                //price line
                var price = priceList.GetPrice(benex, billingDate);
                //build billing item
                var bi = new BillingItem
                {
                    PartitionKey = $"{activeList.CustomerCode}-{activeList.Year}-{activeList.Month}",
                    RowKey = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.Now,
                    Beneficiary = $"{benex.NationalId} {benex.Name}",
                    ProductCode = benex.ProductCode,
                    Amount = Convert.ToDouble(price)
                };
                billingItems.Add(bi);
            }

            return billingItems;
        }
    }
}
