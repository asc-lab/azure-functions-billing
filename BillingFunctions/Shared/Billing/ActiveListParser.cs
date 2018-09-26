using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace BillingFunctions
{
    public static class ActiveListParser
    {
        public static ActiveList Parse(CloudBlockBlob myBlob)
        {
            var parts = myBlob.Name.Split(new char[] { '_' });
            return new ActiveList
            {
                CustomerCode = parts[0],
                Year = int.Parse(parts[1]),
                Month = int.Parse(parts[2]),
                DataLines = myBlob.DownloadTextAsync().Result.Split(Environment.NewLine.ToCharArray()) //FIXME async!
            };
        }
    }

    public class ActiveList
    {
        public string CustomerCode { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string[] DataLines { get; set; }
    }
}
