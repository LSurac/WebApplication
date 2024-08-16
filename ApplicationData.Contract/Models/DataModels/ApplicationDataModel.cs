namespace WebApplication.ApplicationData.Contract.Models.DataModels
{
    public class ApplicationDataModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public List<ApplicantDataModel>? ApplicantList { get; set; }
    }
}
