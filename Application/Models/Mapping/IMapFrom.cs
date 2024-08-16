using AutoMapper;

namespace WebApplication.Application.Models.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(
            Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}
