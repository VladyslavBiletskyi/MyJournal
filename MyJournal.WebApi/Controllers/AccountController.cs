using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility;
using MyJournal.WebApi.Models.Account;

namespace MyJournal.WebApi.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager userManager;

        public AccountController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model.NewPassword != model.NewPasswordConfirmation)
            {
                ModelState.AddModelError(nameof(model.NewPasswordConfirmation), "Пароли не совпадают");
                return View();
            }
            var login = User.Claims.FirstOrDefault(x => x.Type == Constants.UserLoginClaimName)?.Value;
            var user = userManager.TryAuthenticate(login, model.OldPassword, out bool isUserFound);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.OldPassword), "Пароль не верен");
                return View();
            }

            if (userManager.ChangePassword(login, model.NewPassword))
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(nameof(model.OldPassword), "Ошибка при смене пароля");
            return View();
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
                    new Claim(Constants.UserLoginClaimName, user.Login),
                    new Claim(Constants.RoleClaimName, user.Role),
                    new Claim("name", $"{user.FirstName} {user.LastName}")
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
                if (!isUserFound)
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
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Register([FromForm]RegisterModel model)
        {
            var result = userManager.Create(model.Login, model.Password, model.FirstName, model.LastName, model.Surname, model.GroupId, model.IsTeacher);
            if (!result.IsValid)
            {
                ModelState.AddModelError(nameof(model.Login), result.ValidationMessages.FirstOrDefault());
                return View();
            }

            return RedirectToAction("Index", "Home");
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