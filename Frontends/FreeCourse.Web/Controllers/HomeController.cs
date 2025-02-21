using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICatelogService _catelogService;

        public HomeController(ILogger<HomeController> logger, ICatelogService catelogService)
        {
            _logger = logger;
            _catelogService = catelogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catelogService.GetAllCourseAsync());
        }

        public async Task<IActionResult> Detail(string id)
        {
            return View(await _catelogService.GetByCourseId(id));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
            {
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
