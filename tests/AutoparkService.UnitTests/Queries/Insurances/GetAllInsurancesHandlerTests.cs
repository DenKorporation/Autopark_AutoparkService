using AutoMapper;
using AutoparkService.Application.DTOs.Insurance.Response;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Insurances.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Insurances;

[TestSubject(typeof(GetAllInsurancesHandler))]
public class GetAllInsurancesHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<InsuranceProfile>()));

    private readonly Mock<IInsuranceRepository> _insuranceRepositoryMock = new Mock<IInsuranceRepository>();
    private readonly GetAllInsurancesHandler _handler;

    public GetAllInsurancesHandlerTests()
    {
        _handler = new GetAllInsurancesHandler(
            _mapperMock,
            _insuranceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NonEmptyCollection_ReturnsOkWithPagedList()
    {
        // Arrange
        var command = new GetAllInsurancesQuery(
            new FilterInsurancesRequest(
                1,
                5,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null));

        const int listSize = 10;
        var insuranceListQueryable = InsuranceDataFaker
            .InsuranceResponseFaker
            .Generate(listSize);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetAllAsync(
                        1,
                        5,
                        It.IsAny<Func<IQueryable<Insurance>, IQueryable<InsuranceResponse>>>(),
                        It.IsAny<Specification<Insurance>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insuranceListQueryable);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(listSize);
    }
}
