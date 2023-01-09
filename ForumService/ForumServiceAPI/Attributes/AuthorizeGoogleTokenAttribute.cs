using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForumServiceAPI.Attributes;

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
                var tokenString = stringValues.ToString().Replace("Bearer ", "");
                _logger.LogInformation("Parsing token: {TokenString}", tokenString);
                var userDetails = await GoogleJsonWebSignature.ValidateAsync(tokenString);
                if (userDetails.Audience.ToString() != _configuration["Google:ClientId"])
                {
                    _logger.LogInformation("Token audience {Audience} not valid", userDetails.Audience.ToString());
                    context.Result = new UnauthorizedResult();
                    return;
                }
                int userId = int.Parse(userDetails.Subject);
                string username = userDetails.Name;
                if (username == "")
                {
                    _logger.LogInformation("Token username {Username} not valid", username);
                    context.Result = new UnauthorizedResult();
                    return;
                }

                _logger.LogInformation("Token is valid");
                context.HttpContext.Items.Add("UserId", userId);
                context.HttpContext.Items.Add("Username", username);
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