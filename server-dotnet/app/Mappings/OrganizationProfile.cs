using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet_dal.Entities;

namespace server_dotnet.Mappings;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationDto>();
        CreateMap<OrganizationCreateUpdateDto, Organization>();
    }
}
