using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        await app.RunAsync();
    }
}
