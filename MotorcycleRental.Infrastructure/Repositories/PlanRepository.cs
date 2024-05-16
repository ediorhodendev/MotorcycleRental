using MongoDB.Driver;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;

namespace MotorcycleRental.Infrastructure.Repositories
{

    public class PlanRepository : IPlanRepository
    {
        private readonly IMongoCollection<Plan> _plans;

        public PlanRepository(IMongoDatabase database)
        {
            _plans = database.GetCollection<Plan>("Plans");
        }

        public async Task<List<Plan>> GetAllPlansAsync()
        {
            return await _plans.Find(_ => true).ToListAsync();
        }

        public async Task<Plan> GetPlanByIdAsync(string id)
        {
            return await _plans.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddPlanAsync(Plan plan)
        {
            await _plans.InsertOneAsync(plan);
        }

        public async Task UpdatePlanAsync(Plan plan)
        {
            await _plans.ReplaceOneAsync(p => p.Id == plan.Id, plan);
        }

        public async Task DeletePlanAsync(string id)
        {
            await _plans.DeleteOneAsync(p => p.Id == id);
        }
    }
}