using MediatR;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;

namespace MotorcycleRental.Application.Handlers
{
    public class MotorcycleHandlers :
        IRequestHandler<CreateMotorcycleCommand, string>,
        IRequestHandler<UpdateMotorcycleCommand, string>,
        IRequestHandler<DeleteMotorcycleCommand, string>,
        IRequestHandler<GetMotocycleByIdQuery, Motorcycle>,
        IRequestHandler<GetMotorcycleByLicensePlateQuery, Motorcycle>,
        
        IRequestHandler<GetAllMotorcyclesQuery, List<Motorcycle>>
    {
        private readonly IMotorcycleRepository _repository;
        private readonly ILogger<MotorcycleHandlers> _logger;

        public MotorcycleHandlers(IMotorcycleRepository repository, ILogger<MotorcycleHandlers> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var motorcycle = new Motorcycle { Year = request.Year, Model = request.Model, LicensePlate = request.Plate };
                await _repository.AddMotorcycleAsync(motorcycle);
                return motorcycle.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating motorcycle");
                throw;
            }
        }

        public async Task<string> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var motorcycle = new Motorcycle { Id = request.Id, LicensePlate = request.NewLicensePlate };
                await _repository.UpdateMotorcycleAsync(request.Id, motorcycle);
                return request.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motorcycle with ID: {MotorcycleId}", request.Id);
                throw;
            }
        }

        public async Task<string> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteMotorcycleAsync(request.Id);
                return request.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting motorcycle with ID: {MotorcycleId}", request.Id);
                throw;
            }
        }

        public async Task<List<Motorcycle>> Handle(GetAllMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return (List< Motorcycle>)await _repository.GetAllMotorcyclesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all motorcycles");
                throw;
            }
        }

        public async Task<Motorcycle> Handle(GetMotocycleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _repository.GetMotorcycleByIdAsync(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving motorcycle by ID: {MotorcycleId}", request.Id);
                throw;
            }
        }

        public async Task<Motorcycle> Handle(GetMotorcycleByLicensePlateQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetMotorcycleByLicensePlateAsync(request.LicensePlate);
        }
    }
}
