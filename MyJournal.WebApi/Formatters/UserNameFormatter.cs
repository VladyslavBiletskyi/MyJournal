using MyJournal.Domain.Entities;
using MyJournal.WebApi.Extensibility.Formatters;

namespace MyJournal.WebApi.Formatters
{
    public class UserNameFormatter : IUserNameFormatter
    {
        public string FormatFull(ApplicationUser user)
        {
            return $"{user.Surname} {FormatShort(user)}";
        }

        public string FormatShort(ApplicationUser user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
    }
}
