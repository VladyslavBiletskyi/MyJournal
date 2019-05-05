using System.Collections.Generic;
using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Services
{
    public interface IMessageService
    {
        int GetUnreadMessagesCount(ApplicationUser user);

        IEnumerable<Message> GetSentMessages(ApplicationUser user);

        IEnumerable<Message> GetReceivedMessages(ApplicationUser user);

        bool SendMessage(Message message);

        void SetRead(Message message);

        Message Get(int messageId);
    }
}