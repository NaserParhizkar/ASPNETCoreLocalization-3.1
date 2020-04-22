using Microsoft.AspNetCore.Mvc;

namespace Localization.Web.UI.Areas.Administrator.Controllers
{
    public class HomeController : Infrastructure.BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}