
using MongoDB.Driver;
using Moq;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Infrastructure.Repositories;
using Xunit;

namespace MotorcycleRental.Tests.Repository
{

    public class RentalRepositoryTests
    {
        private readonly Mock<IMongoCollection<Rental>> _mockRentalsCollection;
        private readonly RentalRepository _rentalRepository;
        private readonly Mock<IMongoDatabase> _mockDatabase;

        public RentalRepositoryTests()
        {
            _mockRentalsCollection = new Mock<IMongoCollection<Rental>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockDatabase.Setup(m => m.GetCollection<Rental>("Rentals", null)).Returns(_mockRentalsCollection.Object);

            _rentalRepository = new RentalRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task AddRentalAsync_AddsRentalSuccessfully()
        {
            var rental = new Rental { Id = "1", MotorcycleId = "Moto1" };
            _mockRentalsCollection.Setup(x => x.InsertOneAsync(rental, null, default))
                                  .Returns(Task.CompletedTask)
                                  .Verifiable();

            await _rentalRepository.AddRentalAsync(rental);

            _mockRentalsCollection.Verify();
        }


       

        [Fact]
        public async Task UpdateRentalAsync_ThrowsIfRentalNotFound()
        {
            var rental = new Rental { Id = "1", MotorcycleId = "Moto1" };
            _mockRentalsCollection.Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Rental>>(),
                It.IsAny<Rental>(), It.IsAny<ReplaceOptions>(), default))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(0, 0, null));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _rentalRepository.UpdateRentalAsync(rental));
        }

        [Fact]
        public async Task DeleteRentalAsync_ThrowsIfRentalNotFound()
        {
            var rentalId = "1";
            _mockRentalsCollection.Setup(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Rental>>(), default))
                .ReturnsAsync(new DeleteResult.Acknowledged(0));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _rentalRepository.DeleteRentalAsync(rentalId));
        }
    }
}