using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly IIdentityService _identityService;

        [BindProperty]
        public string Token { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; }

        public ResetPasswordModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult OnGet(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                UserId = userId;
                Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _identityService.ResetPasswordAsync(UserId, Token, NewPassword);
                if (result.Succeeded)
                    return RedirectToPage("/Login");
                else
                    return Page();
            }
            return Page();
        }
    }
}
