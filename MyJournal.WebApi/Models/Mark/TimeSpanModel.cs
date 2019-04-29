using System;
using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Mark
{
    public class TimeSpanModel
    {
        [Required (ErrorMessage = "Дата повинна бути вказана")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Дата повинна бути вказана")]
        public DateTime DateTo { get; set; }
    }
}
