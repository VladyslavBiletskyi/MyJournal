using MyJournal.Domain.Entities;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Extensibility
{
    public interface IUserManager
    {
        ApplicationUser TryAuthenticate(string login, string password, out bool isUserFound);

        ValidationResult<ApplicationUser> Create(string login, string password, string firstName, string lastName, string surname, int groupId, bool isTeacher);
    }
}
