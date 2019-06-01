using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Providers;

namespace MyJournal.WebApi
{
    public class UpdateActivityFilter : IAuthorizationFilter
    {
        private readonly IUserService userService;
        private readonly ICurrentUserProvider currentUserProvider;

        public UpdateActivityFilter(IUserService userService, ICurrentUserProvider currentUserProvider)
        {
            this.userService = userService;
            this.currentUserProvider = currentUserProvider;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user != null && user.Claims.Any())
            {
                var currentUser = currentUserProvider.GetCurrentUser<ApplicationUser>(user);
                if (currentUser != null)
                {
                    currentUser.LastActivity = DateTime.Now;
                    userService.Update(currentUser);
                }
            }
        }
    }
}
