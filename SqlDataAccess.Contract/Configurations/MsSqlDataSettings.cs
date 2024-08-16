namespace WebApplication.SqlDataAccess.Contract.Configurations
{
    public class MsSqlDataSettings
    {
        public const string SectionName = "MsSqlDataSettings";

        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
    }
}
