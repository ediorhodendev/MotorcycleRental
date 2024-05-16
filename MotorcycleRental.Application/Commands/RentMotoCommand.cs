using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class RentMotorcycleCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Delivery person ID is required.")]
        public string DeliveryPersonId { get; set; }

        [Required(ErrorMessage = "Motorcycle ID is required.")]
        public string MotorcycleId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Start date must be a valid date.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "End date must be a valid date.")]
        public DateTime EndDate { get; set; }
    }
}
