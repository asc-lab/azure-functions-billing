using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;

namespace BillingFunctions
{
    class PriceRepository
    {
        private readonly DocumentClient client;

        public PriceRepository(
            string connStr = "https://localhost:8081/",
            string authKey = "")
        {
            client = new DocumentClient(new Uri(connStr), authKey);
        }

        public static PriceRepository Connect(
            string connStr = "https://localhost:8081/",
            string authKey = "")
        {
            return new PriceRepository(connStr, authKey);
        }

        public PriceList GetPriceList(string customerCode)
        {
            var priceList = client.CreateDocumentQuery<PriceList>(
                UriFactory.CreateDocumentCollectionUri("crm", "prices"))
                .Where(p => p.CustomerCode == "ASC");

            return priceList.ToList()[0];
        }
    }
}
