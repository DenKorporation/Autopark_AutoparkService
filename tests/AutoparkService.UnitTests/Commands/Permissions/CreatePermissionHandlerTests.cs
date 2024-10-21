using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Request;
using AutoparkService.Application.Errors;
using AutoparkService.Application.Errors.Base;
using AutoparkService.Application.Errors.Vehicle;
using AutoparkService.Application.UseCases.Commands.Permissions.Create;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Commands.Permissions;

[TestSubject(typeof(CreatePermissionHandler))]
public class CreatePermissionHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new Mock<IPermissionRepository>();
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock = new Mock<IVehicleRepository>();
    private readonly CreatePermissionHandler _handler;

    public CreatePermissionHandlerTests()
    {
        _handler = new CreatePermissionHandler(
            _permissionRepositoryMock.Object,
            _vehicleRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotExist_ReturnsVehicleNotFoundError()
    {
        // Arrange
        var command = new CreatePermissionCommand(PermissionDataFaker.PermissionRequestFaker.Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<VehicleNotFoundError>();
        _permissionRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Permission>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_PermissionForVehicleAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var permission = PermissionDataFaker
            .PermissionFaker
            .RuleFor(p => p.VehicleId, _ => vehicle.Id)
            .Generate();

        var command = new CreatePermissionCommand(
            PermissionDataFaker
                .PermissionRequestFaker
                .RuleFor(p => p.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByVehicleId(
                        vehicle.Id,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _permissionRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Permission>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_PermissionWithGivenNumberAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var permission = PermissionDataFaker.PermissionFaker.Generate();

        var command = new CreatePermissionCommand(
            PermissionDataFaker
                .PermissionRequestFaker
                .RuleFor(p => p.Number, _ => permission.Number)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetByNumberAsync(
                        command.Request.Number,
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<ConflictError>();
        _permissionRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Permission>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesFailure_ReturnsInternalServerError()
    {
        // Arrange
        var permission = PermissionDataFaker.PermissionFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreatePermissionCommand(
            PermissionDataFaker
                .PermissionRequestFaker
                .RuleFor(p => p.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<Permission>(
                        It.IsAny<PermissionRequest>()))
            .Returns(permission);

        _permissionRepositoryMock.Setup(
                x =>
                    x.CreateAsync(
                        It.IsAny<Permission>(),
                        It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<InternalServerError>();
        _permissionRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Permission>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SaveChangesSuccess_ReturnsOkWithValue()
    {
        // Arrange
        var permission = PermissionDataFaker.PermissionFaker.Generate();
        var vehicle = VehicleDataFaker.VehicleFaker.Generate();
        var command = new CreatePermissionCommand(
            PermissionDataFaker
                .PermissionRequestFaker
                .RuleFor(p => p.VehicleId, _ => vehicle.Id)
                .Generate());

        _vehicleRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(
                        It.IsAny<Guid>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        _mapperMock
            .Setup(
                x =>
                    x.Map<Permission>(
                        It.IsAny<PermissionRequest>()))
            .Returns(permission);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _permissionRepositoryMock.Verify(
            x => x.CreateAsync(
                It.IsAny<Permission>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
