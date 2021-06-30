using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<LogoutModel> _logger;

        public CreateModel(IRoleService roleService, ILogger<LogoutModel> logger)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRoleAsync(Input.Name);
                if (result.Succeeded)
                {
                    _logger.LogInformation("{Role} role was created", Input.Name);
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogError("Error occured creating role {Role}", Input.Name);
                    return Page();
                }
            }
            return Page();
        }
    }
}
