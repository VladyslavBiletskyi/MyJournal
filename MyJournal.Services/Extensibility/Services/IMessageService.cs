using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Services
{
    public interface IMessageService
    {
        int GetUnreadMessagesCount(ApplicationUser user);
    }
}