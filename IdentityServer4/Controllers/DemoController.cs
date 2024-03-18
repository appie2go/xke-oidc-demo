using Microsoft.AspNetCore.Mvc;

namespace TestIdentityServer.Controllers;

public class DemoController : Controller
{
    [HttpGet("redirect")]
    public IActionResult Get()
    {
        return View("Redirect");
    }
}