using Moq;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Handlers;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;
using Xunit;

namespace MotorcycleRental.Tests.Handlers
{

    public class RentalHandlersTests
    {
        private readonly Mock<IRentalRepository> _mockRentalRepository;
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly Mock<IDeliveryRepository> _mockDeliveryRepository;
        private readonly RentalHandlers _handler;

        public RentalHandlersTests()
        {
            _mockRentalRepository = new Mock<IRentalRepository>();
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
            _mockDeliveryRepository = new Mock<IDeliveryRepository>();
            _handler = new RentalHandlers(_mockRentalRepository.Object, _mockMotorcycleRepository.Object, _mockDeliveryRepository.Object);
        }

        [Fact]
        public async Task Handle_GetAllRentalsQuery_ReturnsAllRentals()
        {
            var query = new GetAllRentalsQuery();
            var rentals = new List<Rental>
        {
            new Rental { Id = "1" }
        };

            _mockRentalRepository.Setup(x => x.GetAllRentalsAsync()).ReturnsAsync(rentals);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
        }

        [Fact]
        public async Task Handle_UpdateRentalCommand_UpdatesRentalSuccessfully()
        {
            var command = new UpdateRentalCommand
            {
                Id = "1",
                MotorcycleId = "2",
                DeliveryPersonId = "2",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3)
            };
            var rental = new Rental { Id = "1" };

            _mockRentalRepository.Setup(x => x.GetRentalByIdAsync("1")).ReturnsAsync(rental);
            _mockRentalRepository.Setup(x => x.UpdateRentalAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("1", result);
        }

        [Fact]
        public async Task Handle_DeleteRentalCommand_DeletesRentalSuccessfully()
        {
            var command = new DeleteRentalCommand { Id = "1" };

            _mockRentalRepository.Setup(x => x.DeleteRentalAsync("1")).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("1", result);
        }

        [Fact]
        public async Task Handle_CreateRentalCommand_ThrowsIfMotorcycleNotFound()
        {
            var command = new CreateRentalCommand
            {
                MotorcycleId = "1",
                DeliveryPersonId = "1",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            _mockMotorcycleRepository.Setup(x => x.GetMotorcycleByIdAsync("1")).ReturnsAsync((Motorcycle)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}