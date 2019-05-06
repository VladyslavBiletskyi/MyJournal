using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Formatters;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Models.Message;

namespace MyJournal.WebApi.Controllers
{
    public class MessageController : Controller
    {
        private ICurrentUserProvider currentUserProvider;
        private IMessageService messageService;
        private IUserNameFormatter userNameFormatter;
        private IUserService userService;

        public MessageController(ICurrentUserProvider currentUserProvider, IMessageService messageService, IUserNameFormatter userNameFormatter, IUserService userService)
        {
            this.currentUserProvider = currentUserProvider;
            this.messageService = messageService;
            this.userNameFormatter = userNameFormatter;
            this.userService = userService;
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

            var sentMessages = messageService.GetSentMessages(user).ToList();
            var receivedMessages = messageService.GetReceivedMessages(user).ToList();

            return View(new MessageListModel
            {
                SentMessages = sentMessages.Select(x => ConvertToClientModelAndCrop(x, true)).ToList(),
                ReceivedMessages = receivedMessages.Select(x => ConvertToClientModelAndCrop(x, true)).ToList()
            } );
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

        [HttpGet]
        [Authorize]
        public IActionResult Display(int messageId)
        {
            var message = messageService.Get(messageId);
            if (message == null)
            {
                return RedirectToAction("Index");
            }

            var currentUser = currentUserProvider.GetCurrentUser<ApplicationUser>(User);
            if (!(message.Addressee.Id == currentUser.Id || message.Sender.Id == currentUser?.Id))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (message.Addressee.Id == currentUser.Id && !message.Read)
            {
                messageService.SetRead(message);
            }

            return View(ConvertToClientModelAndCrop(message, false));
        }

        [HttpGet]
        [Authorize]
        public IActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult NewMessage([FromForm]MessageModel model)
        {
            var addressee = userService.FindUser(model.AddresseeId);
            var currentUser = currentUserProvider.GetCurrentUser<ApplicationUser>(User);
            if (addressee == null || currentUser == null)
            {
                ModelState.AddModelError(nameof(model.AddresseeId), "Користувач не знайдений");
                return View();
            }
            if (String.IsNullOrEmpty(model.Text))
            {
                ModelState.AddModelError(nameof(model.AddresseeId), "Неможливо надіслати пустого листа");
            }
            if (addressee.Id == currentUser.Id)
            {
                ModelState.AddModelError(nameof(model.AddresseeId), "Неможливо надіслати листа собі");
            }

            var message = new Message
            {
                Addressee = addressee,
                DateTime = DateTime.Now,
                Read = false,
                Sender = currentUser,
                Text = model.Text
            };
            if (!messageService.SendMessage(message))
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        private MessageModel ConvertToClientModelAndCrop(Message message, bool shouldCrop)
        {
            return new MessageModel
            {
                Text = shouldCrop && message.Text.Length > 50 ? message.Text.Substring(0, 47) + "..." : message.Text,
                AddresseeId = message.Addressee.Id,
                AddresseeName = userNameFormatter.FormatShort(message.Addressee),
                SenderId = message.Sender.Id,
                SenderName = userNameFormatter.FormatShort(message.Sender),
                Read = message.Read,
                DateTime = message.DateTime,
                Id = message.Id
            };
        }
    }
}