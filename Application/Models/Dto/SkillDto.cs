using AutoMapper;
using WebApplication.Application.Models.Mapping;
using WebApplication.ApplicationData.Contract.Models.DataModels;

namespace WebApplication.Application.Models.Dto
{
    public class SkillDto : IMapFrom<SkillDataModel>
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool? IsCurrent { get; set; }

        public void Mapping(
            Profile profile)
        {
            profile.CreateMap<SkillDataModel, SkillDto>();
        }
    }
}
