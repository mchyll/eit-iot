using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EitIotService.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EitIotService.Controllers
{
    /// <summary>
    /// Controller with a status endpoint used for testing and development.
    /// </summary>
    [EnableCors("AllowAnyOrigin")]
    [Route("api")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService statusService;

        public StatusController(IStatusService statusService)
        {
            this.statusService = statusService;
        }

        // GET: api/Status
        [HttpGet("Status")]
        public string Status()
        {
            var uptime = DateTimeOffset.Now - statusService.StartupTime;
            return $"Running version {statusService.RunningVersion}\n"
                    + $"Uptime: {uptime} (since {statusService.StartupTime:o})\n"
                    + $"Environment: {statusService.RunningEnvironment}";
        }
    }
}