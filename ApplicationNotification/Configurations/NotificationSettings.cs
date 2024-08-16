namespace WebTimeClock.ApplicationNotification.Configurations
{
    public class NotificationSettings
    {
        public const string SectionName = "NotificationSettings";
        public List<string> NotificationMailAddressList { get; set; }
        public string NotificationMailSubject { get; set; }
        public string NotificationMailBody { get; set; }
    }
}
