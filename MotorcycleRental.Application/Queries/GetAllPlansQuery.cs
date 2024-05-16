using MediatR;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{


    public class GetAllPlansQuery : IRequest<List<Plan>> { }

}
