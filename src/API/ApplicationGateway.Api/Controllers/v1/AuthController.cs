using AuthLibrary.IServices;
using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationGateway.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            _logger.LogInformation($"Authentication Initiated for {request.Username}");
            AuthenticateResponse response = await _authService.AuthenticateAsync(request);
            if (!response.IsAuthenticated)
            {
                _logger.LogError($"Authentication Failed with {response.Message}");
                return Unauthorized(response);
            }
            _logger.LogInformation("Authentication Successful");
            return Ok(response);
        }
    }
}
