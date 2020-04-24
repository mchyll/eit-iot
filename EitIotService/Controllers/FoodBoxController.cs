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
    /// <summary>
    /// This controller represents the publicly consumable web API for interacting with the food box devices,
    /// like getting the latest measurements and reserving a food box.
    /// </summary>
    [EnableCors("AllowAnyOrigin")]
    [Route("api/FoodBox")]
    [ApiController]
    public class FoodBoxController : ControllerBase
    {
        private readonly SensorDataContext _context;

        public FoodBoxController(SensorDataContext context)
        {
            _context = context;
        }

        // GET: api/FoodBox/Measurements/Latest?deviceId=a3b2c1
        /// <summary>
        /// Returns the single latest measurement datapoint for a food box device.
        /// </summary>
        /// <param name="deviceId">the id of the food box device</param>
        /// <returns></returns>
        [HttpGet("Measurements/Latest")]
        public async Task<ActionResult<Measurement>> GetLatestMeasurement(string deviceId)
        {
            // Use the only device in the DB, since we only have a single prototype
            deviceId ??= await _context.SensorDevices.Select(d => d.DeviceId).FirstOrDefaultAsync();

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

        // GET: api/FoodBox/Measurements?deviceId=a3b2c1
        /// <summary>
        /// Returns a list of measurement datapoints for a food box device.
        /// If <paramref name="from"/> and <paramref name="to"/> aren't specified, all measurements are returned.
        /// </summary>
        /// <param name="deviceId">the id of the food box device</param>
        /// <param name="from">optional date and time for the earliest measurement to get</param>
        /// <param name="to">optional date and time for the latest measurement to get</param>
        /// <returns></returns>
        [HttpGet("Measurements")]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements(string deviceId, DateTimeOffset? from, DateTimeOffset? to)
        {
            // Use the only device in the DB, since we only have a single prototype
            deviceId ??= await _context.SensorDevices.Select(d => d.DeviceId).FirstOrDefaultAsync();

            from ??= DateTimeOffset.MinValue;
            to ??= DateTimeOffset.MaxValue;

            return await _context.SensorDatas.AsNoTracking()
                .Where(s => s.DeviceId == deviceId && s.Timestamp >= from && s.Timestamp <= to)
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

        // GET: api/FoodBox/Reserve?deviceId=a3b2c1
        /// <summary>
        /// Dummy method for reserving a food box for exclusive access by a user.
        /// This method does nothing, but serves to illustrate the concept of reserving food boxes though the API.
        /// </summary>
        /// <param name="deviceId">the id of the food box device</param>
        /// <returns></returns>
        [HttpPost("Reserve")]
        public IActionResult ReserveFoodBox(string deviceId)
        {
            return Ok();
        }
    }
}
