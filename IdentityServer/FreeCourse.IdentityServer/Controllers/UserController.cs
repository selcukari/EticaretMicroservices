using FreeCourse.IdentityServer.Dtos;
using FreeCourse.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = signupDto.UserName,
                    Email = signupDto.Email,
                };

                var result = await _userManager.CreateAsync(user, signupDto.Password);

                if (!result.Succeeded)
                {

                    return BadRequest(new { Errors = result.Errors.Select(x => x.Description).ToList(), StatusCode = 400, isSuccessful = false });
                }

                return new OkObjectResult(new { StatusCode = 200, isSuccessful = true });
            }
            catch(System.Exception ex)
            {
                return BadRequest(new { Errors = ex.Message, StatusCode = 400, isSuccessful = false });
            }
        }

        [HttpGet]
        [Route("getUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

                if (userIdClaim == null) return BadRequest();

                var user = await _userManager.FindByIdAsync(userIdClaim.Value);

                if (user == null) return BadRequest();

                return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email });
            }
            catch
            {
                return BadRequest();

            }
        }
    }
}
