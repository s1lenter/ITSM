using Application.Common.Results;
using Application.DTOs.Assets;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Features.Assets;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepository;

    public AssetService(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Result<AssetResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);

        if (asset == null)
        {
            return Result<AssetResponse>.Failure(Error.NotFound);
        }

        var assetResponse = new AssetResponse()
        {
            Id = asset.Id,
            Name = asset.Name,
            Code = asset.Code
        };

        return Result<AssetResponse>.Success(assetResponse);
    }

    public async Task AddAsync(CreateAssetRequest createAssetRequest, CancellationToken cancellationToken)
    {
        var asset = new Asset()
        {
            Name = createAssetRequest.Name,
            Code = createAssetRequest.Code
        };

        await _assetRepository.AddAsync(asset, cancellationToken);
    }
}
