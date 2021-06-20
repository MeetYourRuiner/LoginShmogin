using System.Security.Claims;

namespace LoginShmogin.Application.Models
{
    public class ExternalLoginContext
    {
        public string ProviderName { get; set; }
        public ClaimsPrincipal Principal { get; set; }
    }
}