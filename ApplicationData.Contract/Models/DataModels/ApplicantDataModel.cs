using WebApplication.ApplicationData.Contract.Models.Enums;

namespace WebApplication.ApplicationData.Contract.Models.DataModels
{
    public class ApplicantDataModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public EApplicationState? ApplicationState { get; set; }
        public List<SkillDataModel>? SkillList { get; set; }
    }
}
