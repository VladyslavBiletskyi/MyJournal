using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.WebApi.Extensibility.Formatters;

namespace MyJournal.WebApi.Formatters
{
    public class UserNameFormatter : IUserNameFormatter
    {
        public string FormatFull(ApplicationUser user)
        {
            return $"{user.Surname} {FormatWithoutSurname(user)}";
        }

        public string FormatWithoutSurname(ApplicationUser user)
        {
            return $"{user.FirstName} {user.LastName}";
        }

        public string FormatShort(ApplicationUser user)
        {
            return $"{user.Surname} {user.FirstName.FirstOrDefault()}. {user.LastName.FirstOrDefault()}.";
        }
    }
}
