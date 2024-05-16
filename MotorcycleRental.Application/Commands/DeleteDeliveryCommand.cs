using MediatR;
namespace MotorcycleRental.Application.Commands
{
    public class DeleteDeliveryCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}