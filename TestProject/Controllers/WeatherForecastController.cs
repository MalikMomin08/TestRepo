using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
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
        private readonly AppDbContext _db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("Test")]
        public ActionResult Add(StudentDto studentDto)
        {
            var data = new Student()
            {
                Name = studentDto.Name,
                Description = studentDto.Description,
                Address = studentDto.Address
            };

            _db.students.Add(data);
            _db.SaveChanges();

            return Ok(data);
        }

        [HttpGet("TestGet")]
        public ActionResult GetStd()
        {
            var res = _db.students.ToList();
            return Ok(res);
        }

        [HttpPost("Update")]
        public ActionResult Update(int id, StudentDto dto)
        {
            var db = _db.students.FirstOrDefault(x => x.Id == id);
            if (db == null)
                return BadRequest("No Student Found");
            var std = new StudentDto()
            {
                Name = dto.Name,
                Address = dto.Address,
                Description = dto.Description,
            };

            var update = _db.Update(std);
            return Ok(update);
        }


    }
}
