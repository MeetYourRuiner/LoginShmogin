using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
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
    public class ResendEmailConfirmation : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ResendEmailConfirmation> _logger;

        public ResendEmailConfirmation(IIdentityService identityService, IEmailSender emailSender, ILogger<ResendEmailConfirmation> logger)
        {
            _logger = logger;
            _identityService = identityService;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
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
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _identityService.GetUserByEmail(Input.Email);
                if (!user.EmailConfirmed)
                {
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
                }
                _logger.LogInformation("Resend email confirmation was requested for {Email}", Input.Email);
                Message = "Recovery message was sent.";
            }
            return Page();
        }
    }
}
