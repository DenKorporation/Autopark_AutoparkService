using AutoMapper;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Permissions.GetById;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Permissions;

[TestSubject(typeof(GetPermissionByIdHandler))]
public class GetPermissionByIdHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PermissionProfile>()));

    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new Mock<IPermissionRepository>();
    private readonly GetPermissionByIdHandler _handler;

    public GetPermissionByIdHandlerTests()
    {
        _handler = new GetPermissionByIdHandler(
            _mapperMock,
            _permissionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_PermissionNotExist_ReturnsPermissionNotFoundError()
    {
        // Arrange
        var command = new GetPermissionByIdQuery(Guid.NewGuid());

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission)null!);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<PermissionNotFoundError>();
        _permissionRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PermissionExist_ReturnsOkWithPermission()
    {
        // Arrange
        var command = new GetPermissionByIdQuery(Guid.NewGuid());
        var permission = PermissionDataFaker.PermissionFaker.RuleFor(p => p.Id, _ => command.Id);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _permissionRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
