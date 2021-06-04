using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ISignInService signInService, ILogger<LogoutModel> logger)
        {
            _signInService = signInService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            await _signInService.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}