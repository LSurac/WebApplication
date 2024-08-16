using System.Net.Mail;
using System.Net;
using WebTimeClock.ApplicationNotification.Configurations;

namespace WebTimeClock.ApplicationNotification.Services
{
    public class EmailService(
        SmtpSettings smtpSettings,
        NotificationSettings notificationSettings)
    {
        public Task SendEmail(string mailBody)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpSettings.User, smtpSettings.Password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(smtpSettings.User);

                    foreach (var emailAddress in notificationSettings.NotificationMailAddressList)
                    {
                        mailMessage.To.Add(emailAddress);
                    }

                    mailMessage.Subject = notificationSettings.NotificationMailSubject;
                    mailMessage.Body = mailBody;
                    mailMessage.IsBodyHtml = true;

                    client.Send(mailMessage);

                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception caught in EmailService: {ex.Message}");
            }
        }
    }
}
