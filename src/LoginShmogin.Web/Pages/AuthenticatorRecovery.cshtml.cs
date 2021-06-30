using System.Text;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class AuthenticatorRecoveryModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthenticatorRecoveryModel> _logger;

        public AuthenticatorRecoveryModel(IIdentityService identityService, IEmailSender emailSender, ILogger<AuthenticatorRecoveryModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Email { get; set; }
        }

        [ViewData]
        public string Message { get; set; }

        public IActionResult OnGet(string email)
        {
            Input = new InputModel();
            Input.Email = email;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var emailParams = await _identityService.GenerateAuthenticatorResetTokenAsync(Input.Email);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailParams.token));
            var callbackUrl = Url.Page(
                "ResetAuthenticator",
                pageHandler: null,
                new { userId = emailParams.userId, code = code },
                protocol: Request.Scheme
            );
            var emailRequest = new EmailRequest(
                Input.Email,
                "Disabling authenticator",
                $"To reset your authenticator click <a href='{callbackUrl}'>the link</a>"
            );

            await _emailSender.SendEmailAsync(emailRequest);
            _logger.LogInformation("Authenticator reset was requested for {Email}", Input.Email);
            Message = "Recovery message was sent.";
            return Page();
        }
    }
}
