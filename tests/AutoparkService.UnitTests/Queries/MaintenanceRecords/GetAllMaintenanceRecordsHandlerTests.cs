using AutoMapper;
using AutoparkService.Application.DTOs.MaintenanceRecord.Response;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.MaintenanceRecords;

[TestSubject(typeof(GetAllMaintenanceRecordsHandler))]
public class GetAllMaintenanceRecordsHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MaintenanceRecordProfile>()));

    private readonly Mock<IMaintenanceRecordRepository> _maintenanceRecordRepositoryMock =
        new Mock<IMaintenanceRecordRepository>();

    private readonly GetAllMaintenanceRecordsHandler _handler;

    public GetAllMaintenanceRecordsHandlerTests()
    {
        _handler = new GetAllMaintenanceRecordsHandler(
            _mapperMock,
            _maintenanceRecordRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NonEmptyCollection_ReturnsOkWithPagedList()
    {
        // Arrange
        var command = new GetAllMaintenanceRecordsQuery(
            new FilterMaintenanceRecordsRequest(
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
                null));

        const int listSize = 10;
        var maintenanceRecordListQueryable = MaintenanceRecordDataFaker
            .MaintenanceRecordResponseFaker
            .Generate(listSize);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetAllAsync(
                        1,
                        5,
                        It.IsAny<Func<IQueryable<MaintenanceRecord>, IQueryable<MaintenanceRecordResponse>>>(),
                        It.IsAny<Specification<MaintenanceRecord>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecordListQueryable);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(listSize);
    }
}
