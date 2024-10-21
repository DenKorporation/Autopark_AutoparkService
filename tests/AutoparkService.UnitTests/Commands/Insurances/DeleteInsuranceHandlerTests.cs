using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.UseCases.Commands.Insurances.Delete;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Insurances;

[TestSubject(typeof(DeleteInsuranceHandler))]
public class DeleteInsuranceHandlerTests
{
    private readonly Mock<IInsuranceRepository> _insuranceRepositoryMock = new Mock<IInsuranceRepository>();
    private readonly DeleteInsuranceHandler _handler;

    public DeleteInsuranceHandlerTests()
    {
        _handler = new DeleteInsuranceHandler(
            _insuranceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InsuranceNotExist_ReturnsInsuranceNotFoundError()
    {
        // Arrange
        var command = new DeleteInsuranceCommand(Guid.NewGuid());

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Insurance)null!);

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
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new DeleteInsuranceCommand(insurance.Id);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(
                        insurance,
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _insuranceRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _insuranceRepositoryMock.Verify(
            x => x.DeleteAsync(
                insurance,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteSuccess_ReturnsOk()
    {
        // Arrange
        var insurance = InsuranceDataFaker.InsuranceFaker.Generate();
        var command = new DeleteInsuranceCommand(insurance.Id);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(insurance);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _insuranceRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _insuranceRepositoryMock.Verify(
            x => x.DeleteAsync(
                insurance,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
