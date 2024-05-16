using Amazon.S3;
using Moq;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Handlers;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Infrastructure.Repositories;
using Xunit;


namespace MotorcycleRental.Tests.Handlers
{

    public class DeliveryHandlersTests
    {
        private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly Mock<IAmazonS3> _mockAmazonS3;
        
        private readonly DeliveryHandlers _handler;

        public DeliveryHandlersTests()
        {
            _mockDeliveryRepository = new Mock<IDeliveryRepository>();
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
           
            

            _handler = new DeliveryHandlers(
                _mockDeliveryRepository.Object,
                _mockMotorcycleRepository.Object
               
                );
        }

        [Fact]
        public async Task Handle_RegisterDeliveryCommand_ThrowsWhenCNPJExists()
        {
            var command = new RegisterDeliveryCommand
            {
                Name = "John Doe",
                Cnpj = "12345678901234",
                LicenseNumber = "AB123456",
                LicenseType = "A",
                LicenseImageUrl = "path/to/image",
                BirthDate = DateTime.UtcNow
            };

            _mockDeliveryRepository.Setup(x => x.CheckCNPJExistsAsync(command.Cnpj)).ReturnsAsync(true);
            _mockDeliveryRepository.Setup(x => x.CheckLicenseNumberExistsAsync(command.LicenseNumber)).ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_GetAllDeliverysQuery_ReturnsDeliveryList()
        {
            var query = new GetAllDeliverysQuery();
            var deliveries = new List<Delivery>
        {
            new Delivery { Id = "1", Name = "John Doe" }
        };

            _mockDeliveryRepository.Setup(x => x.GetAllDeliverysAsync()).ReturnsAsync(deliveries);

            var result = await _handler.Handle(query, default);

            Assert.Single(result);
            Assert.Equal("John Doe", result[0].Name);
        }

        [Fact]
        public async Task Handle_UpdateDeliveryCommand_UpdatesAndReturnsId()
        {
            var command = new UpdateDeliveryCommand
            {
                Id = "1",
                Name = "Updated Name"
            };

            var delivery = new Delivery { Id = "1", Name = "John Doe" };

            _mockDeliveryRepository.Setup(x => x.GetDeliveryByIdAsync(command.Id)).ReturnsAsync(delivery);
            _mockDeliveryRepository.Setup(x => x.UpdateDeliveryAsync(It.IsAny<Delivery>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, default);

            Assert.Equal("1", result);
            _mockDeliveryRepository.Verify(x => x.UpdateDeliveryAsync(It.Is<Delivery>(d => d.Name == "Updated Name")), Times.Once);
        }

        [Fact]
        public async Task Handle_RentMotorcycleCommand_ThrowsIfDeliveryPersonNotFound()
        {
            var command = new RentMotorcycleCommand
            {
                MotorcycleId = "M1",
                DeliveryPersonId = "D1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            _mockDeliveryRepository.Setup(x => x.GetDeliveryByIdAsync(command.DeliveryPersonId)).ReturnsAsync((Delivery)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        }
    }
}