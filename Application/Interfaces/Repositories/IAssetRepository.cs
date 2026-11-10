using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAssetRepository
{
    public Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task AddAsync(Asset asset, CancellationToken cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
