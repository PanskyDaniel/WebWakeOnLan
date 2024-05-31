using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebWakeOnLan.Models;
using WebWakeOnLan.Shared;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace WebWakeOnLan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WakeController : Controller
    {
        private readonly Wake _wakeService;
        internal ILogger<WakeController> Logger;

        public WakeController(Wake wakeService, ILogger<WakeController> logger)
        {
            _wakeService = wakeService;
            Logger = logger;
        }

        [HttpGet(Name = "Probud")]
        [Route("/[controller]/Probud")]
        [AllowAnonymous]
        public IActionResult Probud([FromQuery] MacAdress pars)
        {
            string mac = pars.Adresa;

            if (!string.IsNullOrWhiteSpace(mac))
            {
                _wakeService.PoslatWakeOnLan(mac);
                Logger.LogInformation("Wake on lan se poslal na {Mac}", mac);
            }
            else
            {
                Logger.LogWarning("Pokus o odeslání  Wake-on-LAN s prázdnou MAC adresou");
            }

            return Ok();
        }
    }
}
