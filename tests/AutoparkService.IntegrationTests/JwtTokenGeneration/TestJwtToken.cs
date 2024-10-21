using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AutoparkService.IntegrationTests.JwtTokenGeneration;

public class TestJwtToken
{
    public List<Claim> Claims { get; } = new();
    public int ExpiresInMinutes { get; set; } = 30;

    public TestJwtToken WithRole(string roleName)
    {
        Claims.Add(new Claim(ClaimTypes.Role, roleName));

        return this;
    }

    public TestJwtToken WithExpiration(int expiresInMinutes)
    {
        ExpiresInMinutes = expiresInMinutes;

        return this;
    }

    public string Build()
    {
        var token = new JwtSecurityToken(
            JwtTokenProvider.Issuer,
            JwtTokenProvider.Issuer,
            Claims,
            expires: DateTime.Now.AddMinutes(ExpiresInMinutes),
            signingCredentials: JwtTokenProvider.SigningCredentials);

        return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(token);
    }
}
