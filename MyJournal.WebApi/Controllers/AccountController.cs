using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility;
using MyJournal.WebApi.Models;

namespace MyJournal.WebApi.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager userManager;

        public AccountController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Login(string returnUrl)
        {
            if (IsUrlValid(returnUrl))
            {
                return View(new LoginModel
                {
                    ReturnUrl = returnUrl
                });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }

            var user = userManager.TryAuthenticate(model.Login, model.Password, out var isUserFound);
            if (user != null)
            {

                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("user", user.Login),
                    new Claim("role", user.Role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), new AuthenticationProperties {IsPersistent = model.Remember});
                if (IsUrlValid(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (isUserFound)
                {
                    ModelState.AddModelError(nameof(model.Login), "Пользователь не найден");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Password), "Пароль не верный");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private bool IsUrlValid(string url)
        {
            return !String.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Relative);
        }
    }
}