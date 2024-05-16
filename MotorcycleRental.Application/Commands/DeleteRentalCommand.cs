using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class DeleteRentalCommand : IRequest<string>
    {
        [Required]
        public string Id { get; set; }
    }
}
