using System.Security.Claims;
using MyJournal.Domain.Entities;

namespace MyJournal.WebApi.Extensibility.Providers
{
    public interface ICurrentUserProvider
    {
        TUser GetCurrentUser<TUser>(ClaimsPrincipal user) where TUser : ApplicationUser;
    }
}
