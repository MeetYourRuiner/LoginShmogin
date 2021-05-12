using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Core.Entities;
using LoginShmogin.Infrastructure.Authentication.Identity;
using LoginShmogin.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    public class IndexModel : PageModel
    {
		private readonly AppDbContext _context;

		public IndexModel(AppDbContext context)
		{
			_context = context;
		}
    	public List<ApplicationUser> Users { get; set; }
		
		public void OnGet()
		{
			Users = _context.Users.ToList();
		}
    }
}
