using MediatR;

namespace MotorcycleRental.Application.Commands
{
    public class CalculateTotalCostCommand : IRequest<decimal>
    {
        public string RentalId { get; set; }
        public DateTime ActualReturnDate { get; set; }
    }
}