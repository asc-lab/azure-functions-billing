using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;

namespace Shared.Pricing
{
    public class PriceRepository
    {
        private readonly DocumentClient client;

        private PriceRepository(string connStr, string authKey)
        {
            client = new DocumentClient(new Uri(connStr), authKey);
        }

        public static PriceRepository Connect(string connStr, string authKey)
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
