using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(IIdentityService identityService, ILogger<ResetPasswordModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Token { get; set; }
            public string UserId { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    UserId = userId,
                    Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _identityService.ResetPasswordAsync(Input.UserId, Input.Token, Input.NewPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID {UserId} reseted his password", Input.UserId);
                    return RedirectToPage("/Login");
                }
                else
                {
                    _logger.LogError("Error occured resetting password of User with ID {UserId}", Input.UserId);
                    return Page();
                }
            }
            return Page();
        }
    }
}
