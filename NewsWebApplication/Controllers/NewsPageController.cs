using Microsoft.AspNetCore.Mvc;

namespace NewsWebApplication.Controllers
{
    public class NewsPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
