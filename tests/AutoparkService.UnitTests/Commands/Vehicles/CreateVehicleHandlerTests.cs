using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Request;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.UseCases.Commands.Vehicles.Create;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Vehicles;

[TestSubject(typeof(CreateVehicleHandler))]
public class CreateVehicleHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly CreateVehicleHandler _handler;

    public CreateVehicleHandlerTests()
    {
        _handler = new CreateVehicleHandler(
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var command = new CreateVehicleCommand(VehicleDataFaker.VehicleRequestFaker.Generate());
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();

        _mapperMock
            .Setup(
                x =>
                    x.Map<Vehicle>(
                        It.IsAny<VehicleRequest>()))
            .Returns(vehicle);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.CreateAsync(
                        It.IsAny<Vehicle>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _vehicleRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Vehicle>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var command = new CreateVehicleCommand(VehicleDataFaker.VehicleRequestFaker.Generate());
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();

        _mapperMock
            .Setup(
                x =>
                    x.Map<Vehicle>(
                        It.IsAny<VehicleRequest>()))
            .Returns(vehicle);

        _vehicleRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
