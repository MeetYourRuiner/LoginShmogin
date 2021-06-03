using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Core.DTOs;
using LoginShmogin.Core.Interfaces;
using LoginShmogin.Infrastructure.Authentication.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class AccountRecoveryModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        [ViewData]
        public string Message { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public AccountRecoveryModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "ResetPassword",
                        pageHandler: null,
                        new { userId = user.Id, code = code },
                        protocol: Request.Scheme
                    );
                    var emailRequest = new EmailRequest(
                        Email,
                        "Account Recovery",
                        $"To recovery password click <a href='{callbackUrl}'>the link</a>"
                    );

                    await _emailSender.SendEmailAsync(emailRequest);
                }
                Message = "Recovery message was sent.";
            }
            return Page();
        }
    }
}
