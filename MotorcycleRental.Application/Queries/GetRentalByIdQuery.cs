

using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetRentalByIdQuery : IRequest<Rental>
    {
        public string Id { get; set; }
    }
}