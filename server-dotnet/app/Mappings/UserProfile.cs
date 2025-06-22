using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet_dal.Entities;

namespace server_dotnet.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateUpdateDto, User>();
    }
}
