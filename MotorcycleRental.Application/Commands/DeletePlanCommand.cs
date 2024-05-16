using MediatR;

namespace MotorcycleRental.Application.Commands
{
    

    public class DeletePlanCommand : IRequest
    {
        public string Id { get; set; }
    }

}
