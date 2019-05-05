using System.Collections.Generic;
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
            return messageRepository.Instances().Count(x => !x.Read && x.Addressee.Id == user.Id);
        }

        public IEnumerable<Message> GetSentMessages(ApplicationUser user)
        {
            return messageRepository.Instances().Where(x => x.Sender.Id == user.Id).ToList();
        }

        public IEnumerable<Message> GetReceivedMessages(ApplicationUser user)
        {
            return messageRepository.Instances().Where(x => x.Addressee.Id == user.Id).ToList();
        }

        public Message Get(int messageId)
        {
            return messageRepository.Find(messageId);
        }

        public bool SendMessage(Message message)
        {
            if (message.Sender == message.Addressee)
            {
                return false;
            }

            return messageRepository.CreateInstance(message);
        }

        public void SetRead(Message message)
        {
            if (message.Read)
            {
                return;
            }
            var originalMessage = messageRepository.Find(message.Id);
            originalMessage.Read = true;
            messageRepository.TryUpdateInstance(message);
        }
    }
}