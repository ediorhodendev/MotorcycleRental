using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class CreateRentalCommand : IRequest<string>
    {
        [Required]
        public string MotorcycleId { get; set; }

        [Required]
        public string DeliveryPersonId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
