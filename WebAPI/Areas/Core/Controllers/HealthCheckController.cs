using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAPI.Areas.Core
{
    [ApiVersion("1.0")]
    public class HealthCheckController : CoreBaseController
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IConfiguration _iConfiguration;
        public HealthCheckController(ILogger<HealthCheckController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _iConfiguration = configuration;
        }

        [HttpGet]
        [Route(""), MapToApiVersion("1.0")]
        public OkResult Get()
        {
            _logger.LogWarning($"This is HealthCheck {_iConfiguration["Port"]}");
            return Ok();
        }
    }
}
