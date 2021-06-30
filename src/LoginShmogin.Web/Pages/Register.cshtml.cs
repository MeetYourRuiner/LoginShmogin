using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ISignInService _signInService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(IIdentityService identityService, ISignInService signInService, IEmailSender emailSender, ILogger<RegisterModel> logger)
        {
            _logger = logger;
            _signInService = signInService;
            _identityService = identityService;
            _emailSender = emailSender;
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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Compare("Password", ErrorMessage = "The passwords do not match")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            public string PasswordConfirm { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                (_, var result) = await _identityService.CreateUserAsync(Input.Email, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Created new user with email {Email}", Input.Email);

                    var emailParams = await _identityService.GenerateEmailConfirmationTokenAsync(Input.Email);
                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailParams.token));
                    var callbackUrl = Url.Page(
                        "/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = emailParams.userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var emailRequest = new EmailRequest(
                        Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                    );
                    await _emailSender.SendEmailAsync(emailRequest);

                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            return Page();
        }
    }
}
