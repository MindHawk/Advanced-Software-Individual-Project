using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountServiceAPI.Attributes;

public class AuthorizeGoogleTokenAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly ILogger<AuthorizeGoogleTokenAttribute> _logger;
    private readonly IConfiguration _configuration;

     public AuthorizeGoogleTokenAttribute(ILogger<AuthorizeGoogleTokenAttribute> logger, IConfiguration configuration)
     {
         _logger = logger;
         _configuration = configuration;
     }

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var stringValues))
            {
                _logger.LogInformation("Absent token");
                context.Result = new UnauthorizedResult();
            }
            else
            {
                string tokenString = stringValues.ToString().Replace("Bearer ", "");

                JwtSecurityToken token = new JwtSecurityToken(tokenString);
                string googleId = token.Subject;
                string username = token.Claims.First(c => c.Type == "name").Value;
                context.HttpContext.Items.Add("GoogleId", googleId);
                context.HttpContext.Items.Add("Username", username);
                
                _logger.LogInformation("Parsing token: {Token}", tokenString);
                GoogleJsonWebSignature.Payload? userDetails = await GoogleJsonWebSignature.ValidateAsync(tokenString);
                if (userDetails.Audience.ToString() != _configuration["Google:ClientId"])
                {
                    _logger.LogInformation("Token audience {Audience} not valid", userDetails.Audience.ToString());
                    context.Result = new UnauthorizedResult();
                    return;
                }

                googleId = userDetails.Subject;
                username = userDetails.Name;
                if (username == "" || googleId == "")
                {
                    _logger.LogInformation("Token username or subject is empty");
                    context.Result = new UnauthorizedResult();
                    return;
                }

                _logger.LogInformation("Token is valid");
            }
        }
        catch (InvalidJwtException e)
        {
            _logger.LogInformation("Invalid token", e);
            context.Result = new UnauthorizedResult();
        }
        catch (FormatException e)
        {
            _logger.LogError("User id is not a number", e);
            context.Result = new UnauthorizedResult();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to validate token", e);
            context.Result = new UnauthorizedResult();
        }
    }
}