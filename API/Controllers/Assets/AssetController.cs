using Application.DTOs.Assets;
using Application.Features.Assets;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Assets;

[ApiController]
[Route("api/assets")]
public class AssetController : Controller
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _assetService.GetByIdAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAssetRequest request, CancellationToken cancellationToken)
    {
        await _assetService.AddAsync(request, cancellationToken);
        return Created();
    }
}
