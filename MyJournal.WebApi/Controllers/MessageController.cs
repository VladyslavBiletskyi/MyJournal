using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Providers;

namespace MyJournal.WebApi.Controllers
{
    public class MessageController : Controller
    {
        private ICurrentUserProvider currentUserProvider;
        private IMessageService messageService;

        public MessageController(ICurrentUserProvider currentUserProvider, IMessageService messageService)
        {
            this.currentUserProvider = currentUserProvider;
            this.messageService = messageService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var user = currentUserProvider.GetCurrentUser<ApplicationUser>(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        
        [HttpGet]
        [Authorize]
        public int GetUnreadMessagesCount(ClaimsPrincipal userClaims)
        {
            var user = currentUserProvider.GetCurrentUser<ApplicationUser>(userClaims);
            if (user == null)
            {
                return 0;
            }

            return messageService.GetUnreadMessagesCount(user);
        }
    }
}