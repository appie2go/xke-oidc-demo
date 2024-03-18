using Microsoft.AspNetCore.Mvc;

namespace OpenIdDictServer.Controllers;

public class DemoController : Controller
{
    [HttpGet("Redirect")]
    public IActionResult Redirect()
    {
        return View("Redirect");
    }
    
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Redirect("/connect/authorize?client_id=demo&response_type=id_token token&redirect_uri=https://localhost:7185/redirect&scope=openid profile&nonce=234562461246");
    }
}