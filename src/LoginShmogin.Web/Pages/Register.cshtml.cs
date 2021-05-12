using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Infrastructure.Authentication.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public void OnGet()
        {

        }
        
        public void OnPost()
        {
            
        }
        // public async Task<IActionResult> OnPostAsync()
        // {
        //     if (ModelState.IsValid)
        //     {
        //         ApplicationUser newUser = new ApplicationUser
        //         {
        //             Email = Email,
        //             UserName = Email
        //         };
        //         var result = await _userManager.CreateAsync(newUser, Password);
        //         if (result.Succeeded)
        //         {
        //             await _signInManager.SignInAsync(newUser, false);
        //             return RedirectToPage("/Index");
        //         }
        //         else
        //         {
        //             foreach (var error in result.Errors)
        //             {
        //                 ModelState.AddModelError(string.Empty, error.Description);
        //             }
        //         }
        //     }
        //     return Page();
        // }
    }
}
