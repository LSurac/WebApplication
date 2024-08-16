using AutoMapper;
using WebApplication.Application.Models.Mapping;
using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Models.Enums;

namespace WebApplication.Application.Models.Dto
{
    public class ApplicantDto : IMapFrom<ApplicantDataModel>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public EApplicationState? ApplicationState { get; set; }
        public List<SkillDto>? SkillList { get; set; } = new();

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<ApplicantDataModel, ApplicantDto>();
        }
    }
}
