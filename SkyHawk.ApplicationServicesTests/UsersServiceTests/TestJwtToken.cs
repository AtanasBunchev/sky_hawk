using Microsoft.IdentityModel.Tokens;
using SkyHawk.ApplicationServices.Messaging;
using SkyHawk.ApplicationServices.Messaging.Requests;
using SkyHawk.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace SkyHawk.ApplicationServicesTests.UsersServiceTests;

public partial class UsersServiceTests
{
    private void TestJwtToken(User user, string token)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("785e26cdd9464e8b92c0cd41ae8df74c")
        );
        
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = "SkyHawk",
            ValidateAudience = true,
            ValidAudience = "SkyHawk",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        JwtSecurityToken jwt = (JwtSecurityToken) validatedToken;

        string? id = jwt.Claims.SingleOrDefault(u => u.Type == "LoggedUserId")?.Value;
        Assert.NotNull(id);
        Assert.Equal(user.Id, int.Parse(id));
    }
}
