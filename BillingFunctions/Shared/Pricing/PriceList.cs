using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingFunctions
{
    class PriceList
    {
        public string CustomerCode { get; set; }
        public IList<PriceListItem> Prices { get; set; }

        public decimal GetPrice(Beneficiary beneficiary, DateTime date) =>
            Prices
                .Where(p => p.Matches(beneficiary, date))
                .Select(p => p.Price)
                .FirstOrDefault();
    }

    class PriceListItem
    {
        public string ProductCode { get; set; }
        public Gender Gender { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public decimal Price { get; set; }

        public bool Matches(Beneficiary beneficiary, DateTime date) =>
            beneficiary.ProductCode == this.ProductCode
            && beneficiary.Gender == this.Gender
            && beneficiary.AgeAt(date) >= this.AgeFrom
            && beneficiary.AgeAt(date) <= this.AgeTo;
    }
}
