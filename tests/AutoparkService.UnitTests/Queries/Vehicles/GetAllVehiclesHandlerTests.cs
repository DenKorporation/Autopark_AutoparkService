using AutoMapper;
using AutoparkService.Application.DTOs.Vehicle.Response;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Vehicles.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Vehicles;

[TestSubject(typeof(GetAllVehiclesHandler))]
public class GetAllVehiclesHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<VehicleProfile>()));

    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly GetAllVehiclesHandler _handler;

    public GetAllVehiclesHandlerTests()
    {
        _handler = new GetAllVehiclesHandler(
            _mapperMock,
            _vehicleRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NonEmptyCollection_ReturnsOkWithPagedList()
    {
        // Arrange
        var command = new GetAllVehiclesQuery(
            new FilterVehiclesRequest(
                1,
                5,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null));

        const int listSize = 10;
        var vehicleListQueryable = VehicleDataFaker
            .VehicleResponseFaker
            .Generate(listSize);

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetAllAsync(
                        1,
                        5,
                        It.IsAny<Func<IQueryable<Vehicle>, IQueryable<VehicleResponse>>>(),
                        It.IsAny<Specification<Vehicle>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicleListQueryable);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(listSize);
    }
}
