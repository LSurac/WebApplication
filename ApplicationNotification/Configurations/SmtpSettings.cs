namespace WebTimeClock.ApplicationNotification.Configurations
{
    public class SmtpSettings
    {
        public const string SectionName = "SmtpSettings";
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
