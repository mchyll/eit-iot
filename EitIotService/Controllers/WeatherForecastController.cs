using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElliotLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EitIotService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly DbContext dbContext;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, DbContext dbContext)
		{
			_logger = logger;
			this.dbContext = dbContext;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("Data")]
		public string GetData()
		{
			return dbContext.GetData();
		}

		[HttpGet("Status")]
		public string GetStatus()
		{
			return dbContext.GetData();
		}
	}
}
