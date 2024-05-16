using MongoDB.Driver;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Menssaging;
using MotorcycleRental.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly IMongoCollection<Motorcycle> _motorcycles;
        private readonly IMongoCollection<Rental> _rentals;
        private readonly IKafkaProducer _kafkaProducer;

        public MotorcycleRepository(IMongoDatabase database, IKafkaProducer kafkaProducer)
        {
            _motorcycles = database.GetCollection<Motorcycle>("Motorcycles");
            _rentals = database.GetCollection<Rental>("Rentals");
            _kafkaProducer = kafkaProducer; 
        }

        public async Task AddMotorcycleAsync(Motorcycle motorcycle)
        {
            if (await CheckLicensePlateExistsAsync(motorcycle.LicensePlate))
            {
                throw new InvalidOperationException("A motorcycle with the same license plate already exists.");
            }

            await _motorcycles.InsertOneAsync(motorcycle);
            var eventMessage = $"New Motorcycle Registered: {motorcycle.Id} with year {motorcycle.Year}";
            await _kafkaProducer.SendMessageAsync(eventMessage);
        }


        public async Task DeleteMotorcycleAsync(string motorcycleId)
        {
            if (await HasActiveRentals(motorcycleId))
            {
                throw new InvalidOperationException("Cannot delete the motorcycle as it has active rentals.");
            }
            var deleteResult = await _motorcycles.DeleteOneAsync(m => m.Id == motorcycleId);
            if (deleteResult.DeletedCount == 0)
            {
                throw new KeyNotFoundException("Motorcycle not found with ID: " + motorcycleId);
            }
        }

        public async Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync()
        {
            try
            {
                return await _motorcycles.Find(_ => true).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Failed to connect to the database.");
            }
        }

        public async Task<Motorcycle> GetMotorcycleByIdAsync(string motorcycleId)
        {
            var motorcycle = await _motorcycles.Find(m => m.Id == motorcycleId).FirstOrDefaultAsync();
            if (motorcycle == null)
            {
                throw new KeyNotFoundException("Motorcycle not found with ID: " + motorcycleId);
            }
            return motorcycle;
        }

        public async Task UpdateMotorcycleAsync(string motorcycleId, Motorcycle updatedMotorcycle)
        {
            var updateResult = await _motorcycles.ReplaceOneAsync(m => m.Id == motorcycleId, updatedMotorcycle);
            if (updateResult.ModifiedCount == 0)
            {
                throw new KeyNotFoundException("Motorcycle not found with ID: " + motorcycleId);
            }
        }

        public async Task<bool> CheckLicensePlateExistsAsync(string licensePlate)
        {
            var count = await _motorcycles.CountDocumentsAsync(m => m.LicensePlate == licensePlate);
            return count > 0;
        }

        public async Task RentMotorcycleAsync(string motorcycleId, string deliveryPersonId, DateTime startDate, DateTime endDate)
        {
            if (await HasConflictingRentals(motorcycleId, startDate, endDate))
            {
                throw new InvalidOperationException("Motorcycle is already rented for the requested dates.");
            }
            var rental = new Rental
            {
                MotorcycleId = motorcycleId,
                DeliveryPersonId = deliveryPersonId,
                StartDate = startDate,
                EndDate = endDate,
                DailyRate = CalculateDailyRate(startDate, endDate)
            };
            await _rentals.InsertOneAsync(rental);
        }

        private decimal CalculateDailyRate(DateTime startDate, DateTime endDate)
        {
            var totalDays = (endDate - startDate).Days + 1;
            return totalDays <= 7 ? 30m :
                   totalDays <= 15 ? 28m :
                   totalDays <= 30 ? 22m :
                   totalDays <= 45 ? 20m : 18m;
        }

        private async Task<bool> HasActiveRentals(string motorcycleId)
        {
            var rentalCount = await _rentals.CountDocumentsAsync(r => r.MotorcycleId == motorcycleId && r.EndDate >= DateTime.Now);
            return rentalCount > 0;
        }

        public async Task<Motorcycle> GetMotorcycleByLicensePlateAsync(string licensePlate)
        {
            return await _motorcycles.Find(m => m.LicensePlate == licensePlate).FirstOrDefaultAsync();
        }

        private async Task<bool> HasConflictingRentals(string motorcycleId, DateTime startDate, DateTime endDate)
        {
            var isRented = await _rentals.CountDocumentsAsync(r => r.MotorcycleId == motorcycleId && r.EndDate >= startDate && r.StartDate <= endDate);
            return isRented > 0;
        }
    }
}
