using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;

namespace MyJournal.Services.Services
{
    internal class MessageService: IMessageService
    {
        private IMessageRepository messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public int GetUnreadMessagesCount(ApplicationUser user)
        {
            return messageRepository.Instances().Count(x => x.Addressee == user);
        }
    }
}