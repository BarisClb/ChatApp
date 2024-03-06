using AutoMapper;
using ChatApp.Application.Commands.User;
using ChatApp.Application.Helpers;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Queries.User;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Infrastructure.AutoMapper
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<UserRegisterCommand, User>().ForMember(user => user.Password, options => options.MapFrom(command => SecurityHelper.GetSha256Hash(command.Password)))
                                                  .ForMember(user => user.UserStatus, options => options.MapFrom(x => UserStatusType.Pending))
                                                  .ForMember(user => user.Status, options => options.MapFrom(x => EntityStatusType.Active));
            CreateMap<GetUserByIdForTokenQueryData, GenerateTokenUserInfo>().ReverseMap();
        }
    }
}
