using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Models;

namespace MyJournal.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }

            var user = await userManager.FindByEmailAsync(model.Login);
            if (user != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
                if (isPasswordValid)
                {
                    await signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "Пароль не верный");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.Login), "Пользователь не найден");
            }

            return View();
        }
    }
}