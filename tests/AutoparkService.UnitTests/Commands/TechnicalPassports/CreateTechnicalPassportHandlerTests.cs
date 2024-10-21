using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Create;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.TechnicalPassports;

[TestSubject(typeof(CreateTechnicalPassportHandler))]
public class CreateTechnicalPassportHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

    private readonly Mock<ITechnicalPassportRepository> _technicalPassportRepositoryMock =
        new Mock<ITechnicalPassportRepository>();

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly CreateTechnicalPassportHandler _handler;

    public CreateTechnicalPassportHandlerTests()
    {
        _handler = new CreateTechnicalPassportHandler(
            _technicalPassportRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command =
            new CreateTechnicalPassportCommand(TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

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
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportForVehicleAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker
            .TechnicalPassportFaker
            .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
            .Generate();

        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByVehicleId(
                        vehicle.Id,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenNumberAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();

        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.Number, _ => technicalPassport.Number)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByNumberAsync(
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenVINAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();

        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VIN, _ => technicalPassport.VIN)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByVinAsync(
                        command.Request.VIN,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenLicensePlateAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();

        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.LicensePlate, _ => technicalPassport.LicensePlate)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByLicensePlateAsync(
                        command.Request.LicensePlate,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
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
                    x.Map<TechnicalPassport>(
                        It.IsAny<TechnicalPassportRequest>()))
            .Returns(technicalPassport);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.CreateAsync(
                        It.IsAny<TechnicalPassport>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateTechnicalPassportCommand(
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
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
                    x.Map<TechnicalPassport>(
                        It.IsAny<TechnicalPassportRequest>()))
            .Returns(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _technicalPassportRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
