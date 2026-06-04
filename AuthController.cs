using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        //Registr,login,logout

    
        public AuthController(UserManager<IdentityUser>userManager,RoleManager<IdentityRole> roleManager,SignInManager<IdentityUser>signInManager )
        {
            _userManager=userManager;
            _roleManager=roleManager;
            _signInManager=signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //check for validation error
            if (ModelState.IsValid)
            {

                //create identity user object
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                //create user
                var result=  await _userManager.CreateAsync(user, model.Password);
                //if user created succesfully
                if (result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync("User"))

                    {
                      await  _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    else
                    {
                      await   _userManager.AddToRoleAsync(user,"User");
                      await  _signInManager.SignInAsync(user,true);
                    }
                    return RedirectToAction("Index", "Home");

                }

            }
            return View(model);
          
        }
        
    }
    
}
