using BlazorApp1.Application.Services;

namespace BlazorApp1.WebAPI.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? TenantId => _httpContextAccessor?.HttpContext?.User?.FindFirst("tenant")?.Value;
}