using Microsoft.AspNetCore.Mvc;

namespace EduBroadcast.Controllers
{
        public class HomeController : Controller
        {
                public IActionResult Index()
                {
                        return View();
                }
        }
}


