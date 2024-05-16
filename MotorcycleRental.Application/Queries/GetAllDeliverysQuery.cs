using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetAllDeliverysQuery : IRequest<List<Delivery>>
    {
    }
}