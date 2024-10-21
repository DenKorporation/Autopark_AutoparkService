using AutoMapper;
using AutoparkService.Application.DTOs.Permission.Response;
using AutoparkService.Application.MappingProfiles;
using AutoparkService.Application.UseCases.Queries.Permissions.GetAll;
using AutoparkService.Domain.Models;
using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.UnitTests.DataGenerators;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;

namespace AutoparkService.UnitTests.Queries.Permissions;

[TestSubject(typeof(GetAllPermissionsHandler))]
public class GetAllPermissionsHandlerTests
{
    private readonly IMapper _mapperMock =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<PermissionProfile>()));

    private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new Mock<IPermissionRepository>();
    private readonly GetAllPermissionsHandler _handler;

    public GetAllPermissionsHandlerTests()
    {
        _handler = new GetAllPermissionsHandler(
            _mapperMock,
            _permissionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NonEmptyCollection_ReturnsOkWithPagedList()
    {
        // Arrange
        var command = new GetAllPermissionsQuery(
            new FilterPermissionsRequest(
                1,
                5,
                null,
                null,
                null,
                null,
                null));

        const int listSize = 10;
        var permissionListQueryable = PermissionDataFaker
            .PermissionResponseFaker
            .Generate(listSize);

        _permissionRepositoryMock.Setup(
                x =>
                    x.GetAllAsync(
                        1,
                        5,
                        It.IsAny<Func<IQueryable<Permission>, IQueryable<PermissionResponse>>>(),
                        It.IsAny<Specification<Permission>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(permissionListQueryable);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(listSize);
    }
}
