using MongoDB.Driver;
using Moq;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Repositories;
using Xunit;

namespace MotorcycleRental.Tests.Repository
{

    public class DeliveryRepositoryTests
    {
        private readonly Mock<IMongoCollection<Delivery>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly DeliveryRepository _repository;
        private readonly List<Delivery> _deliveryList;

        public DeliveryRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Delivery>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _deliveryList = new List<Delivery>
        {
            new Delivery { Id = "1", Cnpj = "12345678901234", LicenseNumber = "CNH12345" },
            new Delivery { Id = "2", Cnpj = "23456789012345", LicenseNumber = "CNH23456" }
        };

            _mockDatabase.Setup(db => db.GetCollection<Delivery>("Deliverys", null))
                         .Returns(_mockCollection.Object);

            _repository = new DeliveryRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task AddDeliveryAsync_SavesDelivery()
        {
            var delivery = new Delivery { Id = "3", Cnpj = "34567890123456", LicenseNumber = "CNH34567" };
            _mockCollection.Setup(x => x.InsertOneAsync(delivery, null, default(CancellationToken)))
                           .Verifiable();

            await _repository.AddDeliveryAsync(delivery);

            _mockCollection.Verify(x => x.InsertOneAsync(delivery, null, default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task DeleteDeliveryAsync_DeletesCorrectDelivery()
        {
            var deliveryId = "1";
            _mockCollection.Setup(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Delivery>>(), default(CancellationToken)))
                           .ReturnsAsync(new DeleteResult.Acknowledged(1));

            await _repository.DeleteDeliveryAsync(deliveryId);

            _mockCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Delivery>>(), default(CancellationToken)), Times.Once);
        }


        [Fact]
        public async Task UpdateDeliveryAsync_UpdatesCorrectDelivery()
        {
            var updatedDelivery = new Delivery { Id = "1", Cnpj = "UpdatedCNPJ", LicenseNumber = "UpdatedCNH" };
            _mockCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Delivery>>(), updatedDelivery, It.IsAny<ReplaceOptions>(), default(CancellationToken)))
                           .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, updatedDelivery.Id));

            await _repository.UpdateDeliveryAsync(updatedDelivery);

            _mockCollection.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Delivery>>(), updatedDelivery, It.IsAny<ReplaceOptions>(), default(CancellationToken)), Times.Once);
        }

        
    }
}