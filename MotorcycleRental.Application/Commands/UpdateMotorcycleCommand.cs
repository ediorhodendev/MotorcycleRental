using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class UpdateMotorcycleCommand : IRequest<string>
    {
        public string Id { get; set; }

        [StringLength(10, ErrorMessage = "License plate must be up to 10 characters long.")]
        public string NewLicensePlate { get; set; }
    }
}
