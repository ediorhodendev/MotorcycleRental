using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetDeliveryQuery : IRequest<Delivery>
    {
        public string Id { get; set; }
    }
}
