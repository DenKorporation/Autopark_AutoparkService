using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.Insurances.Update;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Insurances;

[TestSubject(typeof(UpdateInsuranceHandler))]
public class UpdateInsuranceHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IInsuranceRepository> _insuranceRepositoryMock = new Mock<IInsuranceRepository>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly UpdateInsuranceHandler _handler;

    public UpdateInsuranceHandlerTests()
    {
        _handler = new UpdateInsuranceHandler(
            _insuranceRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_InsuranceNotExist_ReturnsInsuranceNotFoundError()
    {
        // Arrange
        var command = new UpdateInsuranceCommand(Guid.NewGuid(), InsuranceDataFaker.InsuranceRequestFaker.Generate());

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InsuranceNotFoundError>();
        _insuranceRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InsuranceWithGivenSeriesAndNumberAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new UpdateInsuranceCommand(Guid.NewGuid(), InsuranceDataFaker.InsuranceRequestFaker.Generate());
        var duplicateInsurance = InsuranceDataFaker
            .InsuranceFaker
            .RuleFor(i => i.Series, _ => command.Request.Series)
            .RuleFor(i => i.Number, _ => command.Request.Number)
            .Generate();

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        command.Request.Series,
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(duplicateInsurance);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _insuranceRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new UpdateInsuranceCommand(Guid.NewGuid(), InsuranceDataFaker.InsuranceRequestFaker.Generate());

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        command.Request.Series,
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

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
        _insuranceRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new UpdateInsuranceCommand(Guid.NewGuid(), InsuranceDataFaker.InsuranceRequestFaker.Generate());

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        command.Request.Series,
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<Insurance>(
                        It.IsAny<InsuranceRequest>()))
            .Returns(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.UpdateAsync(
                        It.IsAny<Insurance>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _insuranceRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new UpdateInsuranceCommand(Guid.NewGuid(), InsuranceDataFaker.InsuranceRequestFaker.Generate());

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        command.Request.Series,
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<Insurance>(
                        It.IsAny<InsuranceRequest>()))
            .Returns(insurance);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _insuranceRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
