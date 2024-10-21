using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Insurance;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Insurances.GetById;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Insurances;

[TestSubject(typeof(GetInsuranceByIdHandler))]
public class GetInsuranceByIdHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<InsuranceProfile>()));

    private readonly Mock<IInsuranceRepository> _insuranceRepositoryMock = new Mock<IInsuranceRepository>();
    private readonly GetInsuranceByIdHandler _handler;

    public GetInsuranceByIdHandlerTests()
    {
        _handler = new GetInsuranceByIdHandler(
            _mapperMock,
            _insuranceRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InsuranceNotExist_ReturnsInsuranceNotFoundError()
    {
        // Arrange
        var command = new GetInsuranceByIdQuery(Guid.NewGuid());

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
    public async Task Handle_InsuranceExist_ReturnsOkWithInsurance()
    {
        // Arrange
        var command = new GetInsuranceByIdQuery(Guid.NewGuid());
        var insurance = InsuranceDataFaker.InsuranceFaker.RuleFor(i => i.Id, _ => command.Id);

        _insuranceRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
    }
}
