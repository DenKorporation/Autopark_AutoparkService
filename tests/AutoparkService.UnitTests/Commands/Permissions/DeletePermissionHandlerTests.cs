using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Permission;
using AutoparkService.Application.UseCases.Commands.Permissions.Delete;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Permissions;

[TestSubject(typeof(DeletePermissionHandler))]
public class DeletePermissionHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new Mock<IPermissionRepository>();
    private readonly DeletePermissionHandler _handler;

    public DeletePermissionHandlerTests()
    {
        _handler = new DeletePermissionHandler(
            _permissionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_PermissionNotExist_ReturnsPermissionNotFoundError()
    {
        // Arrange
        var command = new DeletePermissionCommand(Guid.NewGuid());

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
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var permission = PermissionDataFaker.PermissionFaker.Generate();
        var command = new DeletePermissionCommand(permission.Id);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        _permissionRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(
                        permission,
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _permissionRepositoryMock.Verify(
            x => x.GetByIdAsync(
                command.Id,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _permissionRepositoryMock.Verify(
            x => x.DeleteAsync(
                permission,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteSuccess_ReturnsOk()
    {
        // Arrange
        var permission = PermissionDataFaker.PermissionFaker.Generate();
        var command = new DeletePermissionCommand(permission.Id);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
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

        _permissionRepositoryMock.Verify(
            x => x.DeleteAsync(
                permission,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
