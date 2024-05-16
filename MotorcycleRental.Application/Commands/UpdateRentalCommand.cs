using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class UpdateRentalCommand : IRequest<string>
    {
        [Required]
        public string Id { get; set; }

        public string MotorcycleId { get; set; }
        public string DeliveryPersonId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
