using System.Net;
using System.Net.Http.Json;
using API;
using Application.DTOs.Assets;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests;

public class AssetsApiTest
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _client;

    private PostgreSqlTestFixture _postgres = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _postgres = new PostgreSqlTestFixture();
        await _postgres.StartAsync();

        _webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<
                        IDbContextOptionsConfiguration<ApplicationDbContext>>();
                    services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                    services.RemoveAll<ApplicationDbContext>();
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(_postgres.ConnectionString));
                });
            });
    }

    [SetUp]
    public void SetUp()
    {
        _client = _webApplicationFactory.CreateClient();
    }

    [Test]
    public async Task Should_Found()
    {
        using var scope = _webApplicationFactory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var asset = new Asset
        {
            Name = "laptop",
            Code = "123"
        };

        dbContext.Assets.Add(asset);

        var response = await _client.GetAsync($"/api/assets/{asset.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_NotFound_When_NotExistId()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/assets/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should()
    {
        var assetRequest = new CreateAssetRequest()
        {
            Name = "Test Asset",
            Code = "123"
        };
        var response = await _client.PostAsJsonAsync("/api/assets", assetRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _webApplicationFactory.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
