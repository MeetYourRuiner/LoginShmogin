using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Areas.Profile.Pages
{
    public class AuthenticatorModel : PageModel
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly UrlEncoder _urlEncoder;
        private readonly ILogger<AuthenticatorModel> _logger;
        private readonly IIdentityService _identityService;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public AuthenticatorModel(
            ICurrentUserService currentUserService,
            IAuthenticatorService authenticatorService,
            IIdentityService identityService,
            UrlEncoder urlEncoder,
            ILogger<AuthenticatorModel> logger)
        {
            _identityService = identityService;
            _urlEncoder = urlEncoder;
            _logger = logger;
            _authenticatorService = authenticatorService;
            _currentUserService = currentUserService;
        }

        public bool IsAuthenticatorEnabled { get; set; }
        public bool IsMachineRemembered { get; set; }
        public string SharedKey { get; set; }
        public string AuthenticatorUri { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _currentUserService.UserId;
            IsAuthenticatorEnabled = await _authenticatorService.GetAuthenticatorEnabledAsync(userId);
            if (IsAuthenticatorEnabled)
            {
                IsMachineRemembered = await _authenticatorService.IsMachineRememberedAsync(userId);
            }
            else
            {
                await LoadSharedKeyAndQrCodeUriAsync(userId);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _currentUserService.UserId;
            if (String.IsNullOrEmpty(userId))
            {
                return NotFound($"Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(userId);
                return Page();
            }

            // Strip spaces and hypens
            var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _authenticatorService.VerifyAuthenticatorCodeAsync(
                userId, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(userId);
                return Page();
            }

            await _authenticatorService.SetAuthenticatorEnabledAsync(userId, true);

            _logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);
            StatusMessage = "Your authenticator app has been verified.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDisableAuthenticatorAsync()
        {
            var userId = _currentUserService.UserId;
            if (String.IsNullOrEmpty(userId))
            {
                return NotFound($"Unable to load user.");
            }

            await _authenticatorService.SetAuthenticatorEnabledAsync(userId, false);
            await _authenticatorService.ResetAuthenticatorKeyAsync(userId);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", userId);

            // await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostForgetMachineAsync()
        {
            var userId = _currentUserService.UserId;
            if (String.IsNullOrEmpty(userId))
            {
                return NotFound($"Unable to load user.");
            }

            await _authenticatorService.ForgetMachineAsync();
            StatusMessage = "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
            return RedirectToPage();
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(string userId)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _authenticatorService.GetAuthenticatorKeyAsync(userId);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _authenticatorService.ResetAuthenticatorKeyAsync(userId);
                unformattedKey = await _authenticatorService.GetAuthenticatorKeyAsync(userId);
            }

            SharedKey = FormatKey(unformattedKey);

            UserDTO user = await _identityService.GetUserById(userId);
            AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("LoginShmogin"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
