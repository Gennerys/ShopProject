using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace Microsoft.eShopWeb.Infrastructure.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        //"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"
		public Task Execute(string subject, string message, string email, string apiKey = "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")
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("gennerys1515@gmail.com", "JOJO MARKET"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

			return client.SendEmailAsync(msg);

		}
        public Task SendEmailAsync(string email, string subject, string message)
        {
			subject = "Welcome beloved customer!";
			message = "We are happy you've choosen our shop!";
			// TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
			return Execute(subject, message, message);
        }
    }
}
