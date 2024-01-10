using MediatR;
using Order.Application.Dtos;
using Shared.Dtos;

namespace Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
