using AutoMapper;
using ChatApp.Application.Commands.UserToken;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.AutoMapper
{
    public class UserTokenProfile : Profile
    {
        public UserTokenProfile()
        {
            CreateMap<UserTokenCreateCommand, UserToken>().ReverseMap();
        }
    }
}
