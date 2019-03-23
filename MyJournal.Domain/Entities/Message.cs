using System;
using Microsoft.AspNetCore.Identity;

namespace MyJournal.Domain.Entities
{
    public class Message : BaseInstance
    {
        public Teacher Sender { get; set; }

        public IdentityUser Addressee { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public bool Read { get; set; }
    }
}
