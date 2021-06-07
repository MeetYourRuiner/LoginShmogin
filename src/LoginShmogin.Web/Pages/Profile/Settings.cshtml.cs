using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly IIdentityService _identityService;

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmNewPassword { get; set; }

        public SettingsModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
                var result = await _identityService.ChangePasswordAsync(userId, OldPassword, NewPassword);
                if (result.Succeeded)
                {
                    return Page();
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
