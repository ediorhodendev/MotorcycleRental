using MediatR;
namespace MotorcycleRental.Application.Commands
{
    public class UpdatePlanCommand : IRequest
    {
        public string Id { get; set; }
        public int Days { get; set; }
        public decimal CostPerDay { get; set; }
        public double PenaltyPercentage { get; set; }
    }

}

