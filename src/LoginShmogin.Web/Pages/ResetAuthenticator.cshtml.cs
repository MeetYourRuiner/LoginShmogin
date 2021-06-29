using System.Text;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public ResetAuthenticatorModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return BadRequest("A code must be supplied for authenticator reset.");
            }
            else
            {
                var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _identityService.ResetAuthenticatorAsync(userId, token);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Login");
                }
                else
                {
                    return Page();
                }
            }
        }
    }
}
