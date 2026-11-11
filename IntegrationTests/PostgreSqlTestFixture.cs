using Testcontainers.PostgreSql;

namespace IntegrationTests;

public sealed class PostgreSqlTestFixture
{
    private readonly PostgreSqlContainer _postgres;

    public string ConnectionString => _postgres.GetConnectionString();

    public PostgreSqlTestFixture()
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("itsm_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public async Task StartAsync()
    {
        await _postgres.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}
