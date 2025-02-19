using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class AuthController : Controller
    {
        //private readonly IIdentityService _identityService;

        //public AuthController(IIdentityService identityService)
        //{
        //    _identityService = identityService;
        //}
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
