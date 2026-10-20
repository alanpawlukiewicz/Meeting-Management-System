using Microsoft.AspNetCore.Mvc;

namespace MeetingManager.WebApi.Controllers
{
    public class AuthController : DefaultControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
