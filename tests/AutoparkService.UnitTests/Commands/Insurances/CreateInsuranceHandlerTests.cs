using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.Insurances.Create;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Insurances;

[TestSubject(typeof(CreateInsuranceHandler))]
public class CreateInsuranceHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IInsuranceRepository> _insuranceRepositoryMock = new Mock<IInsuranceRepository>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly CreateInsuranceHandler _handler;

    public CreateInsuranceHandlerTests()
    {
        _handler = new CreateInsuranceHandler(
            _insuranceRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new CreateInsuranceCommand(InsuranceDataFaker.InsuranceRequestFaker.Generate());

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
            x => x.CreateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_InsuranceWithGivenSeriesAndNumberAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateInsuranceCommand(
            InsuranceDataFaker
                .InsuranceRequestFaker
                .RuleFor(i => i.Series, _ => insurance.Series)
                .RuleFor(i => i.Number, _ => insurance.Number)
                .RuleFor(i => i.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        insurance.Series,
                        insurance.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _insuranceRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateInsuranceCommand(
            InsuranceDataFaker
                .InsuranceRequestFaker
                .RuleFor(i => i.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

        _mapperMock
            .Setup(
                x =>
                    x.Map<Insurance>(
                        It.IsAny<InsuranceRequest>()))
            .Returns(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.CreateAsync(
                        It.IsAny<Insurance>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _insuranceRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreateInsuranceCommand(
            InsuranceDataFaker
                .InsuranceRequestFaker
                .RuleFor(i => i.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetBySeriesAndNumberAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance?)null);

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
            x => x.CreateAsync(
                It.IsAny<Insurance>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
