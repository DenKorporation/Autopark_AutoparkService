using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetById;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Vehicles;

[TestSubject(typeof(GetVehicleByIdHandler))]
public class GetVehicleByIdHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<VehicleProfile>()));

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly GetVehicleByIdHandler _handler;

    public GetVehicleByIdHandlerTests()
    {
        _handler = new GetVehicleByIdHandler(
            _mapperMock,
            _vehicleRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new GetVehicleByIdQuery(Guid.NewGuid());

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
    public async Task Handle_VehicleExist_ReturnsOkWithVehicle()
    {
        // Arrange
        var command = new GetVehicleByIdQuery(Guid.NewGuid());
        var vehicle = VehicleDataFaker.VehicleFaker.RuleFor(v => v.Id, _ => command.Id);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
    }
}
