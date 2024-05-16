using MongoDB.Driver;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {

        private readonly IMongoDatabase _database;
        private IMongoCollection<Rental> _rentals;
        private IMongoCollection<Plan> _plans;

        public RentalRepository(IMongoDatabase database)
        {
            _database = database;
            _rentals = database.GetCollection<Rental>("Rentals");
            _plans = database.GetCollection<Plan>("Plans");
        }



        public async Task AddRentalAsync(Rental rental)
        {
            await _rentals.InsertOneAsync(rental);
        }
        
        public async Task<Rental> GetRentalByIdAsync(string id)
        {
            return await _rentals.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Rental>> GetAllRentalsAsync()
        {
            return await _rentals.Find(_ => true).ToListAsync();
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            var updateResult = await _rentals.ReplaceOneAsync(r => r.Id == rental.Id, rental);
            if (updateResult.ModifiedCount == 0)
            {
                throw new KeyNotFoundException("No rental found with ID: " + rental.Id);
            }
        }

        public async Task DeleteRentalAsync(string id)
        {
            var deleteResult = await _rentals.DeleteOneAsync(r => r.Id == id);
            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException("No rental found with ID: " + id);
            }
        }

        public async Task<Plan> GetPlanDetailsForRentalAsync(string rentalId)
        {
            var rental = await _rentals.Find(r => r.Id == rentalId).FirstOrDefaultAsync();
            if (rental == null) return null;
            return await _plans.Find(p => p.Id == rental.PlanId).FirstOrDefaultAsync();
        }
    }
}
