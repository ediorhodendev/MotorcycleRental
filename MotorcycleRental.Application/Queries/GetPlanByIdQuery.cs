using MediatR;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{

    public class GetPlanByIdQuery : IRequest<Plan>
    {
        public string Id { get; set; }
    }

}
