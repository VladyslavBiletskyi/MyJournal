using MyJournal.Domain.Entities;

namespace MyJournal.WebApi.Extensibility.Formatters
{
    public interface IUserNameFormatter
    {
        string FormatFull(ApplicationUser user);
        string FormatShort(ApplicationUser user);
    }
}
