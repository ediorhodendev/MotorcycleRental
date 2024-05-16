using MediatR;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;

namespace MotorcycleRental.Application.Handlers
{
    public class RentalHandlers :
        IRequestHandler<CreateRentalCommand, string>,
        IRequestHandler<UpdateRentalCommand, string>,
        IRequestHandler<DeleteRentalCommand, string>,
        IRequestHandler<CalculateTotalCostCommand, Decimal>,
        IRequestHandler<GetAllRentalsQuery, List<Rental>>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public RentalHandlers(IRentalRepository rentalRepository, IMotorcycleRepository motorcycleRepository, IDeliveryRepository deliveryRepository)
        {
            _rentalRepository = rentalRepository;
            _motorcycleRepository = motorcycleRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task<string> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);
            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"No motorcycle found with ID: {request.MotorcycleId}");
                return null;
            }
                

            var deliveryPerson = await _deliveryRepository.GetDeliveryByIdAsync(request.DeliveryPersonId);
            if (deliveryPerson == null)
                throw new KeyNotFoundException($"No delivery person found with ID: {request.DeliveryPersonId}");

            // Validação das datas
            if (request.StartDate < DateTime.Now || request.EndDate <= request.StartDate)
                throw new InvalidOperationException("Invalid date settings for rental.");

            // Validação das datas
            if (request.StartDate < DateTime.Now || request.EndDate <= request.StartDate)
                throw new InvalidOperationException("Invalid date settings for rental.");

            // Calcula a taxa diária e o custo total
            var rentalDays = (request.EndDate - request.StartDate).Days;
            var dailyRate = CalculateDailyRate(rentalDays);
            var totalCost = dailyRate * rentalDays;

            // Cria o objeto de aluguel
            var rental = new Rental
            {
                MotorcycleId = request.MotorcycleId,
                DeliveryPersonId = request.DeliveryPersonId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DailyRate = dailyRate,
                TotalCost = totalCost
            };

            // Adiciona o aluguel ao repositório e retorna o ID
            await _rentalRepository.AddRentalAsync(rental);
            return rental.Id;
        }
        public async Task<Rental> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(request.Id);
            return rental;
        }

        public async Task<string> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(request.Id);
            if (rental == null)
                throw new InvalidOperationException("Rental not found.");

            if (request.MotorcycleId != null) rental.MotorcycleId = request.MotorcycleId;
            if (request.DeliveryPersonId != null) rental.DeliveryPersonId = request.DeliveryPersonId;
            if (request.StartDate.HasValue) rental.StartDate = request.StartDate.Value;
            if (request.EndDate.HasValue) rental.EndDate = request.EndDate.Value;

            await _rentalRepository.UpdateRentalAsync(rental);
            return rental.Id;
        }

        public async Task<string> Handle(DeleteRentalCommand request, CancellationToken cancellationToken)
        {
            await _rentalRepository.DeleteRentalAsync(request.Id);
            return request.Id;
        }

        public async Task<List<Rental>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
        {
            return await _rentalRepository.GetAllRentalsAsync();
        }

        private decimal CalculateDailyRate(int durationDays)
        {
            if (durationDays <= 7) return 30m;
            if (durationDays <= 15) return 28m;
            if (durationDays <= 30) return 22m;
            if (durationDays <= 45) return 20m;
            return 18m;
        }

        public async Task<decimal> Handle(CalculateTotalCostCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(request.RentalId);
            if (rental == null)
                throw new KeyNotFoundException("Rental not found");

            var plan = await _rentalRepository.GetPlanDetailsForRentalAsync(request.RentalId);
            if (plan == null)
                throw new KeyNotFoundException("Plan not found for the rental");

            decimal totalCost = rental.DailyRate * (rental.EndDate - rental.StartDate).Days;
            if (request.ActualReturnDate < rental.PredictedEndDate)
            {
                // Applying penalty for early return
                int daysEarly = (rental.PredictedEndDate - request.ActualReturnDate).Days;
                // Convert the double penalty percentage to decimal before calculation
                decimal penaltyPercentage = (decimal)plan.PenaltyPercentage / 100m;
                totalCost -= rental.DailyRate * daysEarly * penaltyPercentage;
            }
            else if (request.ActualReturnDate > rental.PredictedEndDate)
            {
                // Additional charge for late return
                int daysLate = (request.ActualReturnDate - rental.PredictedEndDate).Days;
                totalCost += daysLate * 50m; // Assuming $50 per additional day, ensure literal is decimal with 'm'
            }

            return totalCost;
        }

    }
}
