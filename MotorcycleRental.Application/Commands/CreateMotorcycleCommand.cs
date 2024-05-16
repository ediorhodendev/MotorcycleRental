using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class CreateMotorcycleCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Identifier is required.")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "License plate is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "License plate must be 1 to 10 characters long.")]
        public string Plate { get; set; }
    }
}
