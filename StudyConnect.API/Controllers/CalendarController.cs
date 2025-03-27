using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase{

  private readonly ILogger<CalendarController> _logger;

    public CalendarController(ILogger<CalendarController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCalendar")]
    public IEnumerable<Calendar> Get()
    {
        return null;
    }

}