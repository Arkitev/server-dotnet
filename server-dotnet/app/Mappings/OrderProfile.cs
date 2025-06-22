using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet_dal.Entities;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization));
        CreateMap<OrderCreateUpdateDto, Order>();
    }
}
