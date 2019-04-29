using System.Linq;
using System.Security.Claims;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Providers;

namespace MyJournal.WebApi.Providers
{
    internal class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IUserService userService;

        public CurrentUserProvider(IUserService userService)
        {
            this.userService = userService;
        }

        public TUser GetCurrentUser<TUser>(ClaimsPrincipal user) where TUser : ApplicationUser
        {
            var login = user.Claims.FirstOrDefault(x => x.Type == Constants.UserLoginClaimName)?.Value;
            return userService.FindUser(login) as TUser;
        }
    }
}
