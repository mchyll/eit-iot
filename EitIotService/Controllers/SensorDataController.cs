using EitIotService.Data;
using EitIotService.Models;
using EitIotService.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EitIotService.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/SensorData")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataContext _context;
        private readonly IUptimeService uptimeService;
        private readonly IWebHostEnvironment environment;

        public SensorDataController(SensorDataContext context, IUptimeService uptimeService, IWebHostEnvironment environment)
        {
            _context = context;
            this.uptimeService = uptimeService;
            this.environment = environment;
        }

        // GET: api/SensorData/Latest
        [HttpGet("Latest")]
        public async Task<ActionResult<SensorData>> GetLatestSensorData()
        {
            return await _context.SensorDatas
                .OrderByDescending(d => d.Timestamp)
                .ThenByDescending(d => d.Id)
                .FirstOrDefaultAsync();
        }

        // GET: api/SensorData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorData>>> GetSensorDatas()
        {
            return await _context.SensorDatas.ToListAsync();
        }

        // GET: api/SensorData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorData>> GetSensorData(long id)
        {
            var sensorData = await _context.SensorDatas.FindAsync(id);

            if (sensorData == null)
            {
                return NotFound();
            }

            return sensorData;
        }

        // GET: api/SensorData/Status
        [HttpGet("Status")]
        public string Status()
        {
            var uptime = DateTimeOffset.Now - uptimeService.StartupTime;
            var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            return $"Running version {version}\n"
                    + $"Uptime: {uptime} (since {uptimeService.StartupTime.ToString("o")})\n"
                    + $"Environment: {environment.EnvironmentName}";
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            //return File("~/index.html", "text/html");
            return Redirect("/");
        }
    }
}
