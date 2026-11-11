using Application.Common.Results;
using Application.DTOs.Assets;

namespace Application.Features.Assets;

public interface IAssetService
{
    public Task<Result<AssetResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task AddAsync(CreateAssetRequest createAssetRequest, CancellationToken cancellationToken);
}
