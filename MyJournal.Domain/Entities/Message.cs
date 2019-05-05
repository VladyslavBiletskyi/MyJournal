using System;

namespace MyJournal.Domain.Entities
{
    public class Message : BaseInstance
    {
        public ApplicationUser Sender { get; set; }

        public ApplicationUser Addressee { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public bool Read { get; set; }
    }
}
