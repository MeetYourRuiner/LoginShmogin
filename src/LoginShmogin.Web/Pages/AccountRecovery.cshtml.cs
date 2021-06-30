using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class AccountRecoveryModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly IIdentityService _identityService;
        private readonly ILogger<AccountRecoveryModel> _logger;

        public AccountRecoveryModel(IIdentityService identityService, IEmailSender emailSender, ILogger<AccountRecoveryModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
            _emailSender = emailSender;
        }

        [ViewData]
        public string Message { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var emailParams = await _identityService.GeneratePasswordResetTokenAsync(Input.Email);
                var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailParams.token));
                var callbackUrl = Url.Page(
                    "ResetPassword",
                    pageHandler: null,
                    new { userId = emailParams.userId, code = code },
                    protocol: Request.Scheme
                );
                var emailRequest = new EmailRequest(
                    Input.Email,
                    "Account Recovery",
                    $"To recovery password click <a href='{callbackUrl}'>the link</a>"
                );

                await _emailSender.SendEmailAsync(emailRequest);
                _logger.LogInformation("Account recovery was requested for {Email}", Input.Email);
                Message = "Recovery message was sent.";
            }
            return Page();
        }
    }
}
