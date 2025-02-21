using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catelogs;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Create()
        {
            var categories = await _catelogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catelogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            courseCreateInput.UserId = _sharedIdentityService.GetUserId;

            var savedCatelog = await _catelogService.CreateCourseAsync(courseCreateInput);

            if (!savedCatelog)
            {
                ViewBag.ErrorMessage = "An error occurred while saving the course. Please try again.";
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        // veriyi cekmek icin
        public async Task<IActionResult> Update(string id)
        {
            var course = await _catelogService.GetByCourseId(id);
            var categories = await _catelogService.GetAllCategoryAsync();

            if (course == null)
            {
                ViewBag.ErrorMessage = "The specified course could not be found. Please try again.";
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Feature = course.Feature,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Picture = course.Picture
            };

            return View(courseUpdateInput);
        }

        [HttpPost]// kayıt etmek icin
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catelogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            var savedCatelog = await _catelogService.UpdateCourseAsync(courseUpdateInput);

            if (!savedCatelog)
            {
                ViewBag.ErrorMessage = "An error occurred while saving the course. Please try again.";

                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var resultCatelog = await _catelogService.DeleteCourseAsync(id);

            if (!resultCatelog)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the course. Please try again.";
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
