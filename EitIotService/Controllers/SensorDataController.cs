using EitIotService.Data;
using EitIotService.Models;
using EitIotService.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EitIotService.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataContext _context;
        private readonly IStatusService statusService;

        public SensorDataController(SensorDataContext context, IStatusService statusService)
        {
            _context = context;
            this.statusService = statusService;
        }

        // GET: api/Measurements/Latest?deviceId=a3b2c1
        [HttpGet("Measurements/Latest")]
        public async Task<ActionResult<Measurement>> GetLatestSensorData(string deviceId)
        {
            deviceId ??= (await _context.SensorDevices.FirstOrDefaultAsync())?.DeviceId;

            return await _context.SensorDatas.AsNoTracking()
                .Where(s => s.DeviceId == deviceId)
                .OrderByDescending(d => d.Timestamp)
                .ThenByDescending(d => d.Id)
                .Select(s => new Measurement
                {
                    DatapointId = s.Id,
                    DeviceId = s.DeviceId,
                    Timestamp = s.Timestamp,
                    // To find fill percentage, we assume here that the container is 50 cm deep
                    FillContentPercentage = s.FillContentMeters * 2,
                    Temperature = s.Temperature
                })
                .FirstOrDefaultAsync();
        }

        // GET: api/Measurements?deviceId=a3b2c1
        [HttpGet("Measurements")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetSensorDatas(string deviceId)
        {
            deviceId ??= (await _context.SensorDevices.FirstOrDefaultAsync())?.DeviceId;

            return await _context.SensorDatas.AsNoTracking()
                .Where(s => s.DeviceId == deviceId)
                .Select(s => new Measurement
                {
                    DatapointId = s.Id,
                    DeviceId = s.DeviceId,
                    Timestamp = s.Timestamp,
                    // To find fill percentage, we assume here that the container is 50 cm deep
                    FillContentPercentage = s.FillContentMeters * 2,
                    Temperature = s.Temperature
                })
                .ToListAsync();
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
