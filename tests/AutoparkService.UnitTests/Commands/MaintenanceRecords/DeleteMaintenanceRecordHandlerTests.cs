using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.MaintenanceRecord;
using AutoparkService.Application.UseCases.Commands.MaintenanceRecords.Delete;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.MaintenanceRecords;

[TestSubject(typeof(DeleteMaintenanceRecordHandler))]
public class DeleteMaintenanceRecordHandlerTests
{
    private readonly Mock<IMaintenanceRecordRepository> _maintenanceRecordRepositoryMock = new Mock<IMaintenanceRecordRepository>();
    private readonly DeleteMaintenanceRecordHandler _handler;

    public DeleteMaintenanceRecordHandlerTests()
    {
        _handler = new DeleteMaintenanceRecordHandler(
            _maintenanceRecordRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_MaintenanceRecordNotExist_ReturnsMaintenanceRecordNotFoundError()
    {
        // Arrange
        var command = new DeleteMaintenanceRecordCommand(Guid.NewGuid());

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
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var command = new DeleteMaintenanceRecordCommand(maintenanceRecord.Id);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(maintenanceRecord);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(
                        maintenanceRecord,
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _maintenanceRecordRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _maintenanceRecordRepositoryMock.Verify(
            x => x.DeleteAsync(
                maintenanceRecord,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteSuccess_ReturnsOk()
    {
        // Arrange
        var maintenanceRecord = MaintenanceRecordDataFaker.MaintenanceRecordFaker.Generate();
        var command = new DeleteMaintenanceRecordCommand(maintenanceRecord.Id);

        _maintenanceRecordRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
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

        _maintenanceRecordRepositoryMock.Verify(
            x => x.DeleteAsync(
                maintenanceRecord,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
