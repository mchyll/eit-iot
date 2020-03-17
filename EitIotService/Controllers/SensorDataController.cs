using EitIotService.Data;
using EitIotService.Models;
using EitIotService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EitIotService.Controllers
{
    [Route("api/SensorData")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDataContext _context;
        private readonly IUptimeService uptimeService;

        public SensorDataController(SensorDataContext context, IUptimeService uptimeService)
        {
            _context = context;
            this.uptimeService = uptimeService;
        }

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

        // PUT: api/SensorData/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensorData(long id, SensorData sensorData)
        {
            if (id != sensorData.Id)
            {
                return BadRequest();
            }

            _context.Entry(sensorData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SensorData
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<SensorData>> PostSensorData(SensorData sensorData)
        {
            _context.SensorDatas.Add(sensorData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensorData", new { id = sensorData.Id }, sensorData);
        }

        // DELETE: api/SensorData/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SensorData>> DeleteSensorData(long id)
        {
            var sensorData = await _context.SensorDatas.FindAsync(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            _context.SensorDatas.Remove(sensorData);
            await _context.SaveChangesAsync();

            return sensorData;
        }

        [HttpGet("Status")]
        public string Status()
        {
            var uptime = DateTimeOffset.Now - uptimeService.StartupTime;
            return $"Running. Uptime: {uptime} (since {uptimeService.StartupTime.ToString("o")})";
        }

        [HttpGet("Test")]
        public SensorDevice Test()
        {
            return new SensorDevice
            {
                DeviceId = "17dh0cf43jg757",
                CollectionId = "17dh0cf43jg03e",
                Imei = "357518080231657",
                Imsi = "242016000001574",
                Tags = JsonSerializer.Serialize(new Dictionary<string, string> { { "name", "Test lol" } }),
                DataPoints = new List<SensorData>()
            };
        }

        private bool SensorDataExists(long id)
        {
            return _context.SensorDatas.Any(e => e.Id == id);
        }
    }
}
