using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.Vehicles.Delete;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Vehicles;

[TestSubject(typeof(DeleteVehicleHandler))]
public class DeleteVehicleHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly DeleteVehicleHandler _handler;

    public DeleteVehicleHandlerTests()
    {
        _handler = new DeleteVehicleHandler(
            _vehicleRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new DeleteVehicleCommand(Guid.NewGuid());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle)null!);

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
        var command = new DeleteVehicleCommand(vehicle.Id);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(
                        vehicle,
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _vehicleRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _vehicleRepositoryMock.Verify(
            x => x.DeleteAsync(
                vehicle,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteSuccess_ReturnsOk()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new DeleteVehicleCommand(vehicle.Id);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _vehicleRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _vehicleRepositoryMock.Verify(
            x => x.DeleteAsync(
                vehicle,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
