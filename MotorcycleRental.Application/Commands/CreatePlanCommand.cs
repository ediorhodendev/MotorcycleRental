using MediatR;


namespace MotorcycleRental.Application.Commands
{

    public class CreatePlanCommand : IRequest<string>
    {
        public int Days { get; set; }
        public decimal CostPerDay { get; set; }
        public double PenaltyPercentage { get; set; }
    }


}