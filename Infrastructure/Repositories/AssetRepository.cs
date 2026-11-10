using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private ApplicationDbContext _context { get; }

    public AssetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Assets.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken)
    {
        await _context.Assets.AddAsync(asset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
