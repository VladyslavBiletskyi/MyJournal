using System.Collections.Generic;

namespace MyJournal.WebApi.Models.Message
{
    public class MessageListModel
    {
        public IList<MessageModel> SentMessages { get; set; }

        public IList<MessageModel> ReceivedMessages { get; set; }
    }
}