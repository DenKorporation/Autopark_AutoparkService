using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Create;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.MaintenanceRecords;

[TestSubject(typeof(CreateMaintenanceRecordHandler))]
public class CreateMaintenanceRecordHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IMaintenanceRecordRepository> _maintenanceRecordRepositoryMock = new Mock<IMaintenanceRecordRepository>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly CreateMaintenanceRecordHandler _handler;

    public CreateMaintenanceRecordHandlerTests()
    {
        _handler = new CreateMaintenanceRecordHandler(
            _maintenanceRecordRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new CreateMaintenanceRecordCommand(MaintenanceRecordDataFaker.MaintenanceRecordRequestFaker.Generate());

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
            x => x.CreateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateMaintenanceRecordCommand(
            MaintenanceRecordDataFaker
                .MaintenanceRecordRequestFaker
                .RuleFor(mr => mr.VehicleId, _ => vehicle.Id)
                .Generate());

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
                    x.CreateAsync(
                        It.IsAny<MaintenanceRecord>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateMaintenanceRecordCommand(
            MaintenanceRecordDataFaker
                .MaintenanceRecordRequestFaker
                .RuleFor(mr => mr.VehicleId, _ => vehicle.Id)
                .Generate());

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
            x => x.CreateAsync(
                It.IsAny<MaintenanceRecord>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
