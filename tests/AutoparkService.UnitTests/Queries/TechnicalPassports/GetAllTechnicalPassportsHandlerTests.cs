using AutoMapper;
using AutoparkService.Application.DTOs.TechnicalPassport.Response;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.TechnicalPassports;

[TestSubject(typeof(GetAllTechnicalPassportsHandler))]
public class GetAllTechnicalPassportsHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TechnicalPassportProfile>()));

    private readonly Mock<ITechnicalPassportRepository> _technicalPassportRepositoryMock =
        new Mock<ITechnicalPassportRepository>();

    private readonly GetAllTechnicalPassportsHandler _handler;

    public GetAllTechnicalPassportsHandlerTests()
    {
        _handler = new GetAllTechnicalPassportsHandler(
            _mapperMock,
            _technicalPassportRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NonEmptyCollection_ReturnsOkWithPagedList()
    {
        // Arrange
        var command = new GetAllTechnicalPassportsQuery(
            new FilterTechnicalPassportsRequest(
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
                null,
                null,
                null,
                null,
                null,
                null));

        const int listSize = 10;
        var technicalPassportListQueryable = TechnicalPassportDataFaker
            .TechnicalPassportResponseFaker
            .Generate(listSize);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetAllAsync(
                        1,
                        5,
                        It.IsAny<Func<IQueryable<TechnicalPassport>, IQueryable<TechnicalPassportResponse>>>(),
                        It.IsAny<Specification<TechnicalPassport>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassportListQueryable);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(listSize);
    }
}
