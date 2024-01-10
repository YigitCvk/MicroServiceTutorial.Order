using AutoMapper;
using Order.Application.Dtos;
using Order.Domain.OrderAggregate;

namespace Order.Application.Mapping
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<AddressDto, AddressDto>().ReverseMap();
        }
    }
}
