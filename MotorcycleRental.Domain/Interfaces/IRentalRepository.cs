using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task AddRentalAsync(Rental rental);
        Task<Rental> GetRentalByIdAsync(string id);
        Task<List<Rental>> GetAllRentalsAsync();
        Task UpdateRentalAsync(Rental rental);
        Task DeleteRentalAsync(string id);
        Task<Plan> GetPlanDetailsForRentalAsync(string rentalId);

    }
}


