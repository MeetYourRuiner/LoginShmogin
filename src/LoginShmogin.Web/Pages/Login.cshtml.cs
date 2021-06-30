using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ISignInService _signInService;
        private readonly IIdentityService _identityService;

        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IIdentityService identityService, ISignInService signInService, ILogger<LoginModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
            _signInService = signInService;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public IActionResult OnGet()
        {
            if (_signInService.IsSignedIn(User))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInService.SignInAsync(Input.Email,
                                   Input.Password, Input.RememberMe);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in.", Input.Email);
                    return LocalRedirect(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "User is locked out. Try again in 1 minute.");
                    return Page();
                }
                else if (result.Is2FARequired)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe, Email = Input.Email });
                }
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
