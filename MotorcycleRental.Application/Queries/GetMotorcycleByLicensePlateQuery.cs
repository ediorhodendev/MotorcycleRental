using MediatR;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Application.Queries
{
    public class GetMotorcycleByLicensePlateQuery : IRequest<Motorcycle>
    {
        public string LicensePlate { get; set; }

        public GetMotorcycleByLicensePlateQuery(string licensePlate)
        {
            LicensePlate = licensePlate;
        }
    }
}