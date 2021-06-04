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
    public class ConfirmEmailModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }
            
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _identityService.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                StatusMessage = "Thank you for confirming your email.";
                await _identityService.AddToRoleAsync(userId, "User");
            }
            else
            {
                StatusMessage = "Error confirming your email.";
            }
            return Page();
        }
    }
}
