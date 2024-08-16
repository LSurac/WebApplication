using AutoMapper;
using WebApplication.Application.Models.Mapping;
using WebApplication.ApplicationData.Contract.Models.DataModels;

namespace WebApplication.Application.Models.Dto
{
    public class ApplicationDto : IMapFrom<ApplicationDataModel>
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public List<ApplicantDto>? ApplicantList { get; set; } = new();

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<ApplicationDataModel, ApplicationDto>();
        }
    }
}
