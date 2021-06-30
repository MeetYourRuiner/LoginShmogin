using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Areas.Profile.Pages
{
    public class LinksModel : PageModel
    {
        private readonly IExternalLoginService _externalLoginService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<LinksModel> _logger;
        public LinksModel(IExternalLoginService externalLoginService, ICurrentUserService currentUserService, ILogger<LinksModel> logger)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _externalLoginService = externalLoginService;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public Dictionary<string, bool> LinksStatus { get; set; } = new Dictionary<string, bool>();

        public async Task OnGetAsync()
        {
            var loginProviders = await _externalLoginService.GetLoginProvidersAsync();
            var userId = _currentUserService.UserId;
            var connectedLoginProviders = await _externalLoginService.GetLoginsAsync(userId);
            foreach (var provider in loginProviders)
            {
                LinksStatus.Add(provider, connectedLoginProviders.Contains(provider));
            }
        }

        public IActionResult OnPostAddLink(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./Links", pageHandler: "AddLinkCallback", values: new { returnUrl });
            var properties = _externalLoginService.CreateAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, new AuthenticationProperties(properties));
        }

        public async Task<IActionResult> OnGetAddLinkCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                StatusMessage = $"Error from external provider: {remoteError}";
                return Page();
            }
            var info = await _externalLoginService.GetCurrentLoginContextAsync();
            if (info == null)
            {
                StatusMessage = "Error loading external login information during confirmation";
                return Page();
            }
            var userId = _currentUserService.UserId;
            var result = await _externalLoginService.AddLoginAsync(userId);
            if (result.Succeeded)
            {
                StatusMessage = $"The {info.ProviderName} account was successfully linked";
                _logger.LogInformation("User with ID '{UserId}' linked {ProviderName} account.", userId, info.ProviderName);
                return RedirectToPage();
            }
            else
            {
                StatusMessage = "An error occured while linking the account";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveLinkAsync(string provider)
        {
            var userId = _currentUserService.UserId;
            var result = await _externalLoginService.RemoveLoginAsync(userId, provider);
            if (result.Succeeded)
            {
                StatusMessage = $"The {provider} account was successfully unlinked";
                _logger.LogInformation("User with ID '{UserId}' unlinked {ProviderName} account.", userId, provider);
                return RedirectToPage();
            }
            else
            {
                StatusMessage = $"An error occured while unlinking the account";
                return Page();
            }
        }
    }
}
