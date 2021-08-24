using API._DTOs;
using API._Entities;
using AutoMapper;

namespace API._Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>();
            CreateMap<Post, PostDto>();
        }
    }
}