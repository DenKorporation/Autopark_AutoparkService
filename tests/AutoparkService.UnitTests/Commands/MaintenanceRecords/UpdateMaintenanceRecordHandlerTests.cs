using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Update;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.MaintenanceRecords;

[TestSubject(typeof(UpdateMaintenanceRecordHandler))]
public class UpdateMaintenanceRecordHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

    private readonly Mock<IMaintenanceRecordRepository> _maintenanceRecordRepositoryMock =
        new Mock<IMaintenanceRecordRepository>();

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly UpdateMaintenanceRecordHandler _handler;

    public UpdateMaintenanceRecordHandlerTests()
    {
        _handler = new UpdateMaintenanceRecordHandler(
            _maintenanceRecordRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_MaintenanceRecordNotExist_ReturnsMaintenanceRecordNotFoundError()
    {
        // Arrange
        var command = new UpdateMaintenanceRecordCommand(
            Guid.NewGuid(),
            MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker.Generate());

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((MaintenanceRecord?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<MaintenanceRecordNotFoundError>();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var command = new UpdateMaintenanceRecordCommand(
            Guid.NewGuid(),
            MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker.Generate());

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecord);

        _vehicleRepositoryMock.Setup(
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
        _maintenanceRecordRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var command = new UpdateMaintenanceRecordCommand(
            Guid.NewGuid(),
            MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker.Generate());

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecord);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<MaintenanceRecord>(
                        It.IsAny<MaintenanceRecordRequest>()))
            .Returns(maintenanceRecord);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.UpdateAsync(
                        It.IsAny<MaintenanceRecord>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var command = new UpdateMaintenanceRecordCommand(
            Guid.NewGuid(),
            MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker.Generate());

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecord);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<MaintenanceRecord>(
                        It.IsAny<MaintenanceRecordRequest>()))
            .Returns(maintenanceRecord);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
