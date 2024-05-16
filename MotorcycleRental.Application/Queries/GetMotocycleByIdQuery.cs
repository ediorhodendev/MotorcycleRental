using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetMotocycleByIdQuery : IRequest<Motorcycle>
    {
        public string Id { get; set; }
    }
}