using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{   // token almak icin istek atıldıgında bu class devreye girer
    public class IdentityResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
         
        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var existUser = await _userManager.FindByNameAsync(context.UserName);

                if (existUser == null)
                {
                    var errors = new Dictionary<string, object>();
                    errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                    context.Result.CustomResponse = errors;

                    return;
                }
                var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

                if (passwordCheck == false)
                {
                    var errors = new Dictionary<string, object>();
                    errors.Add("errors", new List<string> { "Email veya şifreniz yanlış" });
                    context.Result.CustomResponse = errors;

                    return;
                }
                // her sey dogru ise kullanıcı bilgilerini token bilgilerine ekleyerek geri döner
                context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            } catch(System.Exception ex)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Bir hata oluştu" });
                context.Result.CustomResponse = errors;

                return;
            }
            
        }
    }
}
