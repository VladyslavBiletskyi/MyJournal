using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility
{
    public interface IUserManager
    {
        ApplicationUser TryAuthenticate(string login, string password, out bool isUserFound);
    }
}
