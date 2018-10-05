using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using Shared.Printing;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillingFunctions
{
    public static class NotifyInvoiceFunc
    {
        /// <summary>
        /// Test numbers for Twilio service: https://www.twilio.com/docs/iam/test-credentials?code-sample=code-create-a-message&code-language=C%23&code-sdk-version=5.x#test-sms-messages
        /// </summary>
        /// <param name="notificationRequest"></param>
        /// <param name="email"></param>
        /// <param name="sms"></param>
        /// <param name="log"></param>
        [FunctionName("NotifyInvoiceFunc")]
        public static void Run(
            [QueueTrigger("invoice-notification-request")]InvoiceNotificationRequest notificationRequest,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage email,
            [TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "+15005550006")] out CreateMessageOptions sms,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {notificationRequest}");

            email = CreateEmail(notificationRequest);
            sms = CreateSMS(notificationRequest);
        }

        private static SendGridMessage CreateEmail(InvoiceNotificationRequest request)
        {
            var email = new SendGridMessage();

            email.AddTo("asc-lab@altkom.pl");
            email.AddContent("text/html", $"You have new invoice {request.InvoiceForNotification.InvoiceNumber} for {request.InvoiceForNotification.TotalCost.ToString()}.");
            email.SetFrom(new EmailAddress("asc-lab@altkom.pl"));
            email.SetSubject($"New Invoice - {request.InvoiceForNotification.InvoiceNumber}");

            return email;
        }

        private static CreateMessageOptions CreateSMS(InvoiceNotificationRequest request)
        {
            return new CreateMessageOptions(new PhoneNumber("+15005550006"))
            {
                Body = $"You have new invoice {request.InvoiceForNotification.InvoiceNumber} for {request.InvoiceForNotification.TotalCost.ToString()}."
            };
        }
    }
}
