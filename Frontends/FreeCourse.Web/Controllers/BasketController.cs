using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly ICatelogService _catelogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatelogService catelogService, IBasketService basketService)
        {
            _catelogService = catelogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catelogService.GetByCourseId(courseId);

            if (course == null)
            {
                // loglama yapılabilir
                return BadRequest();
            }

            var basketItem = new BasketItemViewModel { CourseId = course.Id, CourseName = course.Name, Price = course.Price };

            var savedBasket = await _basketService.AddBasketItem(basketItem);

            // index sayfasına yönlendirme
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            var result = await _basketService.RemoveBasketItem(courseId);

            if (!result)
            {
                ViewBag.ErrorMessage = "An error occurred while saving the basketitem. Please try again.";
                return BadRequest();
            }

            // index sayfasına yönlendirme
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                // bir requist den digerine veri tasıma
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelApplyDiscount()
        {
            var cancelBasket = await _basketService.CancelApplyDiscount();

            if (!cancelBasket)
            {
                TempData["discountError"] = "An error occurred while canceling the discount. Please try again.";
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
