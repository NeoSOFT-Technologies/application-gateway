using ApplicationGateway.Application.Features.Certificate.Commands.AddCertificate;
using ApplicationGateway.Application.Features.Certificate.Commands.DeleteCertificate;
using ApplicationGateway.Application.Features.Certificate.Queries.GetAllCertificate;
using ApplicationGateway.Application.Features.Certificate.Queries.GetCertificateById;
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

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> AddCertificate([FromForm]AddCertificateCommand addCertificateCommand)
        {
            _logger.LogInformation("AddCertificate controller initiated");
            Guid certId = await _mediator.Send(addCertificateCommand);
            _logger.LogInformation("AddCertificate controller completed");
            return Ok(certId);
        }

        [HttpGet]
        public async Task<IActionResult> GetCertificate(Guid certId)
        {
            _logger.LogInformation("GetCertificate controller initiated");
            GetCertificateByIdDto cert = await _mediator.Send(new GetCertificateByIdQuery() { CertId=certId});
            _logger.LogInformation("GetCertificate controller completed");
            return Ok(cert);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCertificates()
        {
            _logger.LogInformation("GetAllCertificates controller initiated");
            var allCert = await _mediator.Send(new GetAllCertificateQuery());
            _logger.LogInformation("GetAllCertificates controller completed");
            return Ok(allCert);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCertificate(DeleteCertificateCommand getCertificateByIdQuery)
        {
            _logger.LogInformation("DeleteCertificate controller initiated");
            await _mediator.Send(getCertificateByIdQuery);
            _logger.LogInformation("DeleteCertificate controller completed");
            return Ok();
        }
    }
}
