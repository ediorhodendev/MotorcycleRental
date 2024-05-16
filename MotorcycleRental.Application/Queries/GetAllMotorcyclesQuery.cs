using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetAllMotorcyclesQuery : IRequest<List<Motorcycle>>
    {
    }
}
