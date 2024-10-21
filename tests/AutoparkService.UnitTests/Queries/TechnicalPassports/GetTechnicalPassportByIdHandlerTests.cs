using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.TechnicalPassport;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.TechnicalPassports.GetById;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.TechnicalPassports;

[TestSubject(typeof(GetTechnicalPassportByIdHandler))]
public class GetTechnicalPassportByIdHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TechnicalPassportProfile>()));

    private readonly Mock<ITechnicalPassportRepository> _technicalPassportRepositoryMock = new Mock<ITechnicalPassportRepository>();
    private readonly GetTechnicalPassportByIdHandler _handler;

    public GetTechnicalPassportByIdHandlerTests()
    {
        _handler = new GetTechnicalPassportByIdHandler(
            _mapperMock,
            _technicalPassportRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_TechnicalPassportNotExist_ReturnsTechnicalPassportNotFoundError()
    {
        // Arrange
        var command = new GetTechnicalPassportByIdQuery(Guid.NewGuid());

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
    public async Task Handle_TechnicalPassportExist_ReturnsOkWithTechnicalPassport()
    {
        // Arrange
        var command = new GetTechnicalPassportByIdQuery(Guid.NewGuid());
        var technicalPassport = TechnicalPassportDataFaker.TechnicalPassportFaker.RuleFor(tp => tp.Id, _ => command.Id);

        _technicalPassportRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
    }
}
