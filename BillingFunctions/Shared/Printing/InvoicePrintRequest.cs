using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingFunctions
{
    public class InvoicePrintRequest
    {
        public Invoice InvoiceToPrint { get; set; }
    }

    public class InvoiceSmsRequest
    {
        public Invoice InvoiceToSms { get; set; }
    }
}
