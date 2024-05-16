
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Domain.Interfaces
{

    public interface IMotorcycleRepository
    {
        Task<Motorcycle> GetMotorcycleByIdAsync(string motorcycleId);
        Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync();
        Task AddMotorcycleAsync(Motorcycle motorcycle);
        Task UpdateMotorcycleAsync(string motorcycleId, Motorcycle motorcycle);
        Task DeleteMotorcycleAsync(string motorcycleId);
        Task<bool> CheckLicensePlateExistsAsync(string licensePlate);
        Task RentMotorcycleAsync(string motorcycleId, string DeliveryId, DateTime startDate, DateTime endDate);
        Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate);

    }
}
