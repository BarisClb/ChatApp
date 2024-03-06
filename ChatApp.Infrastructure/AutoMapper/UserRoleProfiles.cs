using AutoMapper;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Queries.UserRole;

namespace ChatApp.Infrastructure.AutoMapper
{
    public class UserRoleProfiles : Profile
    {
        public UserRoleProfiles()
        {
            CreateMap<GetUserRoleByUserIdQueryData, GenerateTokenUserRoleInfo>().ReverseMap();
        }
    }
}
