using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoginShmogin.Web.Areas.Profile.Pages
{
    public static class ManageNavPages
    {
        public static string ChangePassword => "ChangePassword";
        public static string Links => "Links";
        public static string Authenticator => "Authenticator";

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);
        public static string LinksNavClass(ViewContext viewContext) => PageNavClass(viewContext, Links);
        public static string AuthenticatorNavClass(ViewContext viewContext) => PageNavClass(viewContext, Authenticator);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
