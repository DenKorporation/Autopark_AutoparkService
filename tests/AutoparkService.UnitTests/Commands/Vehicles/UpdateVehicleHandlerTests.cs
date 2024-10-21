using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.Vehicles.Update;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Vehicles;

[TestSubject(typeof(UpdateVehicleHandler))]
public class UpdateVehicleHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly UpdateVehicleHandler _handler;

    public UpdateVehicleHandlerTests()
    {
        _handler = new UpdateVehicleHandler(_vehicleRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new UpdateVehicleCommand(
            Guid.NewGuid(),
            VehicleDataFaker.VehicleRequestFaker.Generate());

        _vehicleRepositoryMock
            .Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<VehicleNotFoundError>();
        _vehicleRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new UpdateVehicleCommand(
            vehicle.Id,
            VehicleDataFaker.VehicleRequestFaker.Generate());

        _vehicleRepositoryMock
            .Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _vehicleRepositoryMock
            .Setup(
                x =>
                    x.UpdateAsync(
                        It.IsAny<Vehicle>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _vehicleRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Vehicle>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new UpdateVehicleCommand(
            vehicle.Id,
            VehicleDataFaker.VehicleRequestFaker.Generate());

        _vehicleRepositoryMock
            .Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _vehicleRepositoryMock
            .Setup(
                x =>
                    x.UpdateAsync(
                        It.IsAny<Vehicle>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _vehicleRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Vehicle>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
