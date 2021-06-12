using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IRoleService _roleService;

        public CreateModel(IRoleService roleService)
        {
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
                    return RedirectToPage("./Index");
                }
                else
                {

                    return Page();
                }
            }
            return Page();
        }
    }
}
