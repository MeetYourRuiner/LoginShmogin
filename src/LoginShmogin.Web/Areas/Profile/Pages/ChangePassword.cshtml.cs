using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Areas.Profile.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(IIdentityService identityService, ICurrentUserService currentUserService, ILogger<ChangePasswordModel> logger)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Old password")]
            public string OldPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [Required]
            [Compare("NewPassword", ErrorMessage = "The passwords do not match")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            public string ConfirmNewPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string userId = _currentUserService.UserId;
                var result = await _identityService.ChangePasswordAsync(userId, Input.OldPassword, Input.NewPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID '{UserId}' has changed password.", userId);
                    StatusMessage = "Your password was successfully changed";
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
