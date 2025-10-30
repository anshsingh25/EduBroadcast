using Microsoft.AspNetCore.Mvc;

namespace SchoolCommApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}


