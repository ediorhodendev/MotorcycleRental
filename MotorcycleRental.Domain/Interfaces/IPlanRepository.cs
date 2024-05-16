using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Domain.Interfaces
{
    public interface IPlanRepository
    {
        Task<List<Plan>> GetAllPlansAsync();
        Task<Plan> GetPlanByIdAsync(string id);
        Task AddPlanAsync(Plan plan);
        Task UpdatePlanAsync(Plan plan);
        Task DeletePlanAsync(string id);
    }
}
