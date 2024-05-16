using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetAllRentalsQuery : IRequest<List<Rental>> { }
}
