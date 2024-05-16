using MediatR;

namespace MotorcycleRental.Application.Commands
{
    public class DeleteMotorcycleCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}
