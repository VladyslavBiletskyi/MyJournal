using System;
using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Message
{
    public class MessageModel
    {
        public string AddresseeName { get; set; }

        public int AddresseeId { get; set; }

        public string SenderName { get; set; }

        public int SenderId { get; set; }

        [Required(ErrorMessage = "Неможливо відправити пусте повідомлення")]
        public string Text { get; set; }

        public bool Read { get; set; }

        public DateTime DateTime { get; set; }

        public int Id { get; set; }
    }
}
