using System.Globalization;
using MediatR;
using WebTimeClock.ApplicationNotification.Configurations;
using WebTimeClock.ApplicationNotification.Notification;
using WebTimeClock.ApplicationNotification.Services;

namespace WebTimeClock.ApplicationNotification.NotificationHandler
{
    public class ClockOutNotificationHandler(
        NotificationSettings notificationSettings, 
        EmailService emailService)
        : INotificationHandler<ClockOutNotification>
    {
        public async Task Handle(
            ClockOutNotification notification,
            CancellationToken cancellationToken)
        {
            var mailBody = notificationSettings.NotificationMailBody;

            if (mailBody.Contains("[EMPLOYEE_FULLNAME]"))
            {
                mailBody = mailBody.Replace("[EMPLOYEE_FULLNAME]",
                    $"{notification.Employee.ForName} {notification.Employee.LastName}");
            }

            if (mailBody.Contains("[TIMECLOCK_TIME]"))
            {
                var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Time");
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(notification.TimeClock.UtcTime, cetTimeZone);

                mailBody = mailBody.Replace("[TIMECLOCK_TIME]", localTime.ToString(CultureInfo.CurrentCulture));
            }

            await emailService.SendEmail(mailBody); ;
        }
    }
}