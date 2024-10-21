using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.MaintenanceRecords.GetById;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.MaintenanceRecords;

[TestSubject(typeof(GetMaintenanceRecordByIdHandler))]
public class GetMaintenanceRecordByIdHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MaintenanceRecordProfile>()));

    private readonly Mock<IMaintenanceRecordRepository> _maintenanceRecordRepositoryMock =
        new Mock<IMaintenanceRecordRepository>();

    private readonly GetMaintenanceRecordByIdHandler _handler;

    public GetMaintenanceRecordByIdHandlerTests()
    {
        _handler = new GetMaintenanceRecordByIdHandler(
            _mapperMock,
            _maintenanceRecordRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_MaintenanceRecordNotExist_ReturnsMaintenanceRecordNotFoundError()
    {
        // Arrange
        var command = new GetMaintenanceRecordByIdQuery(Guid.NewGuid());

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MaintenanceRecord)null!);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<MaintenanceRecordNotFoundError>();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MaintenanceRecordExist_ReturnsOkWithMaintenanceRecord()
    {
        // Arrange
        var command = new GetMaintenanceRecordByIdQuery(Guid.NewGuid());
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.RuleFor(mr => mr.Id, _ => command.Id);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecord);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
