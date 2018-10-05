using System;
using System.IO;

namespace Shared.Billing
{
    public static class ActiveListParser
    {
        public static ActiveList Parse(string name, Stream myBlob)
        {
            using (StreamReader sr = new StreamReader(myBlob))
            {
                var dataLines = sr.ReadToEnd();
                var parts = name.Split(new char[] { '_' });
                return new ActiveList
                {
                    CustomerCode = parts[0],
                    Year = int.Parse(parts[1]),
                    Month = int.Parse(parts[2]),
                    DataLines = dataLines.Split(Environment.NewLine.ToCharArray())
                };
            }
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
