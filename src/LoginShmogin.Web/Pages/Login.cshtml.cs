using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ISignInService _signInService;
        private readonly IIdentityService _identityService;

        [BindProperty]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public LoginModel(IIdentityService identityService, ISignInService signInService)
        {
            _identityService = identityService;
            _signInService = signInService;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, 
                // set lockoutOnFailure: true
                var result = await _signInService.SignInAsync(Email,
                                   Password, RememberMe);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                // if (result.RequiresTwoFactor)
                // {
                //     return RedirectToPage("./LoginWith2fa", new
                //     {
                //         ReturnUrl = returnUrl,
                //         RememberMe = RememberMe
                //     });
                // }
                // if (result.IsLockedOut)
                // {
                //     return RedirectToPage("./Lockout");
                // }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
