using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
		public IndexModel()
		{
		}
    }
}
