using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Application.Commands
{
    public class RegisterDeliveryCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CNPJ is required.")]
        [RegularExpression("^[0-9]{14}$", ErrorMessage = "CNPJ must be 14 digits.")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "License type is required.")]
        [RegularExpression("^(A|B|AB)$", ErrorMessage = "License type must be A, B, or AB.")]
        public string LicenseType { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime BirthDate { get; set; }

        // Change to IFormFile to handle file uploads
        [Required(ErrorMessage = "A photo of CNH is required.")]
        public IFormFile LicenseImage { get; set; }

        // Custom validation to ensure image format
        public bool IsLicenseImageValid()
        {
            // Check if the file is in PNG or BMP format
            return LicenseImage.ContentType == "image/png" || LicenseImage.ContentType == "image/bmp";
        }
    }
}
