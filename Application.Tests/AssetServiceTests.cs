using Application.Common.Results;
using Application.Features.Assets;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests;

[TestFixture]
public class AssetServiceTests
{
    private Mock<IAssetRepository> _repositoryMock = null!;
    private IAssetService _assetService = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IAssetRepository>();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(assets[0].Id, CancellationToken.None))
            .ReturnsAsync(assets[0]);

        _assetService = new AssetService(_repositoryMock.Object);
    }

    [Test]
    public async Task Should_ReturnAsset_When_SendValidRequest()
    {
        var result = await _assetService.GetByIdAsync(assets[0].Id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        result.Value!.Id.Should().Be(assets[0].Id);
        result.Value.Name.Should().Be(assets[0].Name);
        result.Value.Code.Should().Be(assets[0].Code);
    }

    [Test]
    public async Task Should_ReturnNotFound_When_SendNonExistValue()
    {
        var result = await _assetService.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Error.NotFound);
    }

    private Asset[] assets =
    [
        new()
        {
            Name = "laptop",
            Code = "123"
        },
        new()
        {
            Name = "mouse",
            Code = "1234"
        },
        new()
        {
            Name = "monitor",
            Code = "1235"
        },
        new()
        {
            Name = "keyboard",
            Code = "1236"
        },
        new()
        {
            Name = "headphones",
            Code = "1237"
        }
    ];
}
