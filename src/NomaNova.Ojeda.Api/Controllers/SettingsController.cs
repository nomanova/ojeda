using Microsoft.AspNetCore.Mvc;
using NomaNova.Ojeda.Api.Controllers.Base;

namespace NomaNova.Ojeda.Api.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ApiController
    {
        public SettingsController()
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("test");
        }
    }
}