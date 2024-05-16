using MongoDB.Driver;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;

namespace MotorcycleRental.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly IMongoCollection<Delivery> _Deliverys;

        public DeliveryRepository(IMongoDatabase database)
        {
            _Deliverys = database.GetCollection<Delivery>("Deliverys");
        }

        public async Task AddDeliveryAsync(Delivery Delivery)
        {
            await _Deliverys.InsertOneAsync(Delivery);
        }

        public async Task DeleteDeliveryAsync(string DeliveryId)
        {
            await _Deliverys.DeleteOneAsync(d => d.Id == DeliveryId);
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliverysAsync()
        {
            return await _Deliverys.Find(_ => true).ToListAsync();
        }

        public async Task<Delivery> GetDeliveryByIdAsync(string DeliveryId)
        {
            return await _Deliverys.Find(d => d.Id == DeliveryId).FirstOrDefaultAsync();
        }
      

        public async Task UpdateDeliveryAsync(Delivery Delivery)
        {
            await _Deliverys.ReplaceOneAsync(d => d.Id == Delivery.Id, Delivery);
        }

        public async Task<bool> CheckCNPJExistsAsync(string cnpj)
        {
            var result = await _Deliverys.CountDocumentsAsync(d => d.Cnpj == cnpj);
            return result > 0;
        }

        public async Task<bool> CheckLicenseNumberExistsAsync(string cnhNumber)
        {
            var result = await _Deliverys.CountDocumentsAsync(d => d.LicenseNumber == cnhNumber);
            return result > 0;
        }
    }
}
