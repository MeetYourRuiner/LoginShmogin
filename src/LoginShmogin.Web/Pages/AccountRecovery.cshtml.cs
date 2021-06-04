using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class AccountRecoveryModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly IIdentityService _identityService;

        [ViewData]
        public string Message { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public AccountRecoveryModel(IIdentityService identityService, IEmailSender emailSender)
        {
            _identityService = identityService;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var emailParams = await _identityService.GeneratePasswordResetTokenAsync(Email);
                var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailParams.token));
                var callbackUrl = Url.Page(
                    "ResetPassword",
                    pageHandler: null,
                    new { userId = emailParams.userId, code = code },
                    protocol: Request.Scheme
                );
                var emailRequest = new EmailRequest(
                    Email,
                    "Account Recovery",
                    $"To recovery password click <a href='{callbackUrl}'>the link</a>"
                );

                await _emailSender.SendEmailAsync(emailRequest);
                Message = "Recovery message was sent.";
            }
            return Page();
        }
    }
}
