using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly IExternalLoginService _externalLoginService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            IIdentityService identityService,
            IExternalLoginService externalLoginService,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _identityService = identityService;
            _externalLoginService = externalLoginService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
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

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _externalLoginService.CreateAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, new AuthenticationProperties(properties));
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _externalLoginService.GetExternalServiceInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var result = await _externalLoginService.SignInAsync();
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.ProviderName);
                return LocalRedirect(returnUrl);
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _externalLoginService.GetExternalServiceInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                (string userId, var result) = await _identityService.CreateUserAsync(Input.Email, Input.Password);
                if (result.Succeeded)
                {
                    result = await _externalLoginService.AddExternalLoginAsync(userId);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.ProviderName);

                        (_, var token) = await _identityService.GenerateEmailConfirmationTokenAsync(Input.Email);
                        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                        var callbackUrl = Url.Page(
                            "/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(new EmailRequest(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."));

                        return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            ProviderDisplayName = info.ProviderName;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
