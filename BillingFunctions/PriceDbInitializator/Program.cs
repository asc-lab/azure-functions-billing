using Microsoft.Azure.Documents.Client;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace PriceDbInitializator
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            string configFile = GetConfigFile(args);
            var builder = new ConfigurationBuilder().AddJsonFile(configFile);

            Configuration = builder.Build();

            var client = new DocumentClient(new Uri(Configuration["PriceDbUrl"]), Configuration["PriceDbAuthKey"]);

            AddDoc(client);
            GetDocument(client);

            Console.ReadLine();
        }

        static void GetDocument(DocumentClient client)
        {
            var priceList = client.CreateDocumentQuery<PriceList>(GetDocumentCollectionURI())
                .Where(p => p.CustomerCode == "ASC");

            foreach (var pl in priceList)
            {
                Console.WriteLine($"{pl.CustomerCode}");
                foreach (var pli in pl.Prices)
                {
                    Console.WriteLine($"{pli.ProductCode} {pli.Price}");
                }
            }

        }

        static void AddDoc(DocumentClient client)
        {
            var price = new PriceList
            {
                CustomerCode = "ASC",
                Prices = new List<PriceListItem>
                {
                    new PriceListItem
                    {
                        ProductCode = "A",
                        Gender = Gender.Male,
                        AgeFrom = 0,
                        AgeTo = 65,
                        Price = 200
                    },
                    new PriceListItem
                    {
                        ProductCode = "A",
                        Gender = Gender.Male,
                        AgeFrom = 66,
                        AgeTo = 100,
                        Price = 250
                    },
                    new PriceListItem
                    {
                        ProductCode = "A",
                        Gender = Gender.Female,
                        AgeFrom = 0,
                        AgeTo = 65,
                        Price = 190
                    },
                    new PriceListItem
                    {
                        ProductCode = "A",
                        Gender = Gender.Female,
                        AgeFrom = 66,
                        AgeTo = 100,
                        Price = 240
                    },

                    new PriceListItem
                    {
                        ProductCode = "B",
                        Gender = Gender.Male,
                        AgeFrom = 0,
                        AgeTo = 65,
                        Price = 200
                    },
                    new PriceListItem
                    {
                        ProductCode = "B",
                        Gender = Gender.Male,
                        AgeFrom = 66,
                        AgeTo = 100,
                        Price = 250
                    },
                    new PriceListItem
                    {
                        ProductCode = "B",
                        Gender = Gender.Female,
                        AgeFrom = 0,
                        AgeTo = 65,
                        Price = 190
                    },
                    new PriceListItem
                    {
                        ProductCode = "B",
                        Gender = Gender.Female,
                        AgeFrom = 66,
                        AgeTo = 100,
                        Price = 240
                    },
                }
            };

            client.CreateDocumentAsync(GetDocumentCollectionURI(), price);
        }

        private static string GetConfigFile(string[] args)
        {
            return args.Length > 0 && args[0] != null ? args[0] : "local.appsettings.json";
        }

        private static Uri GetDocumentCollectionURI()
        {
            return UriFactory.CreateDocumentCollectionUri(Configuration["PriceDbName"], Configuration["PriceDbCollection"]);
        }
    }

    class PriceList
    {
        public string CustomerCode { get; set; }
        public IList<PriceListItem> Prices { get; set; }
    }

    class PriceListItem
    {
        public string ProductCode { get; set; }
        public Gender Gender { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public decimal Price { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
