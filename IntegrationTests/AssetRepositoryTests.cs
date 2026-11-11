
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

[TestFixture]
[NonParallelizable]
public class AssetRepositoryTests
{
    private PostgreSqlTestFixture _postgres = null!;
    private DbContextOptions<ApplicationDbContext> _options = null!;
    private ApplicationDbContext _dbContext = null!;
    private IAssetRepository _repository = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _postgres = new PostgreSqlTestFixture();
        await _postgres.StartAsync();

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_postgres.ConnectionString)
            .Options;

        await using var migrationContext = new ApplicationDbContext(_options);

        await migrationContext.Database.MigrateAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        _dbContext = new ApplicationDbContext(_options);

        await ResetDatabaseAsync();

        _repository = new AssetRepository(_dbContext);
    }

    [Test]
    public async Task Should_Add_WhenValidAsset()
    {
        var asset = new Asset
        {
            Name = "laptop",
            Code = "123"
        };

        await _repository.AddAsync(asset, CancellationToken.None);

        var savedAsset = await _dbContext.Assets
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == asset.Id);

        savedAsset.Should().NotBeNull();
        savedAsset!.Code.Should().Be(asset.Code);
        savedAsset.Name.Should().Be(asset.Name);
    }

    [Test]
    public async Task Should_ReturnAsset_WhenExistId()
    {
        var expectedAsset = new Asset
        {
            Name = "mouse",
            Code = "1234"
        };

        await _dbContext.Assets.AddAsync(expectedAsset);
        await _dbContext.SaveChangesAsync();

        var actualAsset = await _repository.GetByIdAsync(
            expectedAsset.Id,
            CancellationToken.None);

        actualAsset.Should().NotBeNull();
        actualAsset!.Id.Should().Be(expectedAsset.Id);
        actualAsset.Code.Should().Be(expectedAsset.Code);
        actualAsset.Name.Should().Be(expectedAsset.Name);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _dbContext.DisposeAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgres.DisposeAsync();
    }

    private async Task ResetDatabaseAsync()
    {
        await _dbContext.Database.ExecuteSqlRawAsync("""TRUNCATE TABLE "Assets" RESTART IDENTITY CASCADE;""");
    }
}
