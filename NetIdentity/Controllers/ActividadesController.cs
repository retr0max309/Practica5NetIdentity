using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetIdentity.Controllers
{
    [Authorize]
    public class ActividadesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "SoloHombres")]
        public IActionResult Deportes()
        {
            return View();
        }

        [Authorize(Policy = "SoloMujeres")]
        public IActionResult Arte()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Tareas()
        {
            return View();
        }
    }
}
