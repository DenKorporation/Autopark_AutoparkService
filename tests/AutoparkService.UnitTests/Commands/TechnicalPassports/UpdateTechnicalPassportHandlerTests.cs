using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Update;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.TechnicalPassports;

[TestSubject(typeof(UpdateTechnicalPassportHandler))]
public class UpdateTechnicalPassportHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

    private readonly Mock<ITechnicalPassportRepository> _technicalPassportRepositoryMock =
        new Mock<ITechnicalPassportRepository>();

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly UpdateTechnicalPassportHandler _handler;

    public UpdateTechnicalPassportHandlerTests()
    {
        _handler = new UpdateTechnicalPassportHandler(
            _technicalPassportRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_TechnicalPassportNotExist_ReturnsTechnicalPassportNotFoundError()
    {
        // Arrange
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((TechnicalPassport?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<TechnicalPassportNotFoundError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenNumberAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        var duplicateTechnicalPassport = TechnicalPassportDataFaker
            .TechnicalPassportFaker
            .RuleFor(tp => tp.Number, _ => command.Request.Number)
            .Generate();

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByNumberAsync(
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateTechnicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenVINAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        var duplicateTechnicalPassport = TechnicalPassportDataFaker
            .TechnicalPassportFaker
            .RuleFor(tp => tp.VIN, _ => command.Request.VIN)
            .Generate();

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByVinAsync(
                        command.Request.VIN,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateTechnicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportWithGivenLicensePlateAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        var duplicateTechnicalPassport = TechnicalPassportDataFaker
            .TechnicalPassportFaker
            .RuleFor(tp => tp.LicensePlate, _ => command.Request.LicensePlate)
            .Generate();

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByLicensePlateAsync(
                        command.Request.LicensePlate,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateTechnicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VehicleId, _ => Guid.NewGuid())
                .Generate());

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

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
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_TechnicalPassportForVehicleAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker
                .TechnicalPassportRequestFaker
                .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
                .Generate());

        var duplicateTechnicalPassport = TechnicalPassportDataFaker
            .TechnicalPassportFaker
            .RuleFor(tp => tp.VehicleId, _ => vehicle.Id)
            .Generate();

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByVehicleId(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateTechnicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

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
                    x.UpdateAsync(
                        It.IsAny<TechnicalPassport>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new UpdateTechnicalPassportCommand(
            Guid.NewGuid(),
            TechnicalPassportDataFaker.TechnicalPassportRequestFaker.Generate());

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

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
            x => x.UpdateAsync(
                It.IsAny<TechnicalPassport>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
