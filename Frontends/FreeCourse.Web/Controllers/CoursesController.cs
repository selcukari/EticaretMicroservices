using FreeCourse.Shared.Services;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatelogService _catelogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatelogService catelogService, ISharedIdentityService sharedIdentityService)
        {
            _catelogService = catelogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catelogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));
        }
    }
}
