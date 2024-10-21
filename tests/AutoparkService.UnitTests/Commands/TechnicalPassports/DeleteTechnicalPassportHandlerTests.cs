using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.UseCases.Commands.TechnicalPassports.Delete;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.TechnicalPassports;

[TestSubject(typeof(DeleteTechnicalPassportHandler))]
public class DeleteTechnicalPassportHandlerTests
{
    private readonly Mock<ITechnicalPassportRepository> _technicalPassportRepositoryMock = new Mock<ITechnicalPassportRepository>();
    private readonly DeleteTechnicalPassportHandler _handler;

    public DeleteTechnicalPassportHandlerTests()
    {
        _handler = new DeleteTechnicalPassportHandler(
            _technicalPassportRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_TechnicalPassportNotExist_ReturnsTechnicalPassportNotFoundError()
    {
        // Arrange
        var command = new DeleteTechnicalPassportCommand(Guid.NewGuid());

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TechnicalPassport)null!);

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
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new DeleteTechnicalPassportCommand(technicalPassport.Id);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(
                        technicalPassport,
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _technicalPassportRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _technicalPassportRepositoryMock.Verify(
            x => x.DeleteAsync(
                technicalPassport,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteSuccess_ReturnsOk()
    {
        // Arrange
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.Generate();
        var command = new DeleteTechnicalPassportCommand(technicalPassport.Id);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(technicalPassport);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _technicalPassportRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _technicalPassportRepositoryMock.Verify(
            x => x.DeleteAsync(
                technicalPassport,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
