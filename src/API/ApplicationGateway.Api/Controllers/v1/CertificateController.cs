using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationGateway.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        readonly ILogger<KeyController> _logger;
        readonly IMediator _mediator;
        public CertificateController(ILogger<KeyController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddCertificate(IFormFile file, string password)
        {
            if (Path.GetExtension(file.FileName).ToLowerInvariant() != ".pfx")
                return BadRequest("Only .pfx file format is allowed");

            string certsPath = @"C:\Users\user\Desktop\CERT CONTROLLER";
            if (!Directory.Exists(certsPath))
            {
                Directory.CreateDirectory(certsPath);
            }
            var filePath = $@"{certsPath}\{Guid.NewGuid()}.pfx";
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }
    }
}
