using System;

namespace Shared.Billing
{
    public class Beneficiary
    {
        public string Name { get; }
        public string NationalId { get; }
        public DateTime BirthDate { get; }
        public Gender Gender { get; }
        public string ProductCode { get; }
        public int AgeAt(DateTime date) => (new DateTime(1, 1, 1) + date.Subtract(BirthDate)).Year - 1;

        public Beneficiary(string nationalId, string name, string productCode)
        {
            Name = name;
            NationalId = nationalId;
            BirthDate = Pesel.BirthdateFromPesel(nationalId);
            Gender = Pesel.GenderFromPesel(nationalId);
            ProductCode = productCode;
        }

        public static Beneficiary FromCsvLine(string csvLine)
        {
            string[] parts = csvLine.Split(new char[] {';'});
            return new Beneficiary(parts[0], parts[1], parts[2]);
        }
    }

    static class Pesel
    {
        public static DateTime BirthdateFromPesel(string id)
        {
            int ctrl = int.Parse(id.Substring(2, 1));

            int year = int.Parse(id.Substring(0, 2));
            year += ctrl <= 1 ? 1900 : 2000;

            int month = int.Parse(id.Substring(3, 1));
            month += ctrl % 2 != 0 ? 10 : 0;

            int day = int.Parse(id.Substring(4, 2));

            return new DateTime(year, month, day);
        }

        public static Gender GenderFromPesel(string id)
        {
            return int.Parse(id.Substring(9, 1)) % 2 == 0 ? Gender.Female : Gender.Male;
        }
    }
}
