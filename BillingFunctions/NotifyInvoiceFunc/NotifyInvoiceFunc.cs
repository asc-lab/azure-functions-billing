using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shared.Printing;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingFunctions
{
    public static class NotifyInvoiceFunc
    {
        [FunctionName("NotifyInvoiceFunc")]
        [return: TwilioSms(AccountSidSetting = "smsAccount", AuthTokenSetting = "smsSecToken", From = "+15005550006")]
        public static CreateMessageOptions Run(
            [QueueTrigger("invoice-sms-request")]InvoiceSmsRequest smsRequest,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {smsRequest}");

            return new CreateMessageOptions(new PhoneNumber("+15005550006"))
            {
                Body = $"You have new invoice {smsRequest.InvoiceToSms.InvoiceNumber} for {smsRequest.InvoiceToSms.TotalCost.ToString()}."
            };
        }
    }
}
