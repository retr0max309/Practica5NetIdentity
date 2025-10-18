using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using NetIdentity.Models;

public class GeneroClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GeneroClaimsTransformation(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity?.IsAuthenticated ?? true) return principal;

        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(id)) return principal;

        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return principal;

        var identity = principal.Identity as ClaimsIdentity;
        if (identity == null) return principal;

        if (!identity.HasClaim(c => c.Type == "genero") && !string.IsNullOrWhiteSpace(user.Genero))
            identity.AddClaim(new Claim("genero", user.Genero));

        if (!identity.HasClaim(c => c.Type == "FechaNacimiento") && user.FechaNacimiento.HasValue)
            identity.AddClaim(new Claim("FechaNacimiento", user.FechaNacimiento.Value.ToString("yyyy-MM-dd")));

        return principal;
    }
}
