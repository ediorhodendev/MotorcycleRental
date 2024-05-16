using Microsoft.Extensions.Logging;
using Moq;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Handlers;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;
using Xunit;

namespace MotorcycleRental.Tests.Handlers
{

    public class MotorcycleHandlersTests
    {
        private readonly Mock<IMotorcycleRepository> _mockRepository;
        private readonly Mock<ILogger<MotorcycleHandlers>> _mockLogger;
        private readonly MotorcycleHandlers _handler;

        public MotorcycleHandlersTests()
        {
            _mockRepository = new Mock<IMotorcycleRepository>();
            _mockLogger = new Mock<ILogger<MotorcycleHandlers>>();
            _handler = new MotorcycleHandlers(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_CreateMotorcycleCommand_ReturnsMotorcycleId()
        {
            var command = new CreateMotorcycleCommand { Year = 2020, Model = "Yamaha R1", Plate = "XYZ123" };
            var motorcycle = new Motorcycle { Id = "1", Year = 2020, Model = "Yamaha R1", LicensePlate = "XYZ123" };

            _mockRepository.Setup(r => r.AddMotorcycleAsync(It.IsAny<Motorcycle>())).Callback((Motorcycle mc) => mc.Id = "1");

            var result = await _handler.Handle(command, new CancellationToken());

            Assert.Equal("1", result);
        }

        [Fact]
        public async Task Handle_UpdateMotorcycleCommand_ReturnsUpdatedMotorcycleId()
        {
            var command = new UpdateMotorcycleCommand { Id = "1", NewLicensePlate = "XYZ456" };
            var motorcycle = new Motorcycle { Id = "1", LicensePlate = "XYZ123" };

            _mockRepository.Setup(r => r.UpdateMotorcycleAsync("1", It.IsAny<Motorcycle>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, new CancellationToken());

            Assert.Equal("1", result);
        }

        [Fact]
        public async Task Handle_DeleteMotorcycleCommand_ReturnsDeletedMotorcycleId()
        {
            var command = new DeleteMotorcycleCommand { Id = "1" };

            _mockRepository.Setup(r => r.DeleteMotorcycleAsync("1")).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, new CancellationToken());

            Assert.Equal("1", result);
        }

        [Fact]
        public async Task Handle_GetAllMotorcyclesQuery_ReturnsAllMotorcycles()
        {
            var query = new GetAllMotorcyclesQuery();
            var motorcycles = new List<Motorcycle> { new Motorcycle { Id = "1", Model = "Yamaha R1" } };

            _mockRepository.Setup(r => r.GetAllMotorcyclesAsync()).ReturnsAsync(motorcycles);

            var result = await _handler.Handle(query, new CancellationToken());

            Assert.Single(result);
            Assert.Equal("Yamaha R1", result[0].Model);
        }

        [Fact]
        public async Task Handle_GetMotorcycleByIdQuery_ReturnsMotorcycle()
        {
            var query = new GetMotocycleByIdQuery { Id = "1" };
            var motorcycle = new Motorcycle { Id = "1", Model = "Yamaha R1" };

            _mockRepository.Setup(r => r.GetMotorcycleByIdAsync("1")).ReturnsAsync(motorcycle);

            var result = await _handler.Handle(query, new CancellationToken());

            Assert.Equal("Yamaha R1", result.Model);
        }

       
    }
}