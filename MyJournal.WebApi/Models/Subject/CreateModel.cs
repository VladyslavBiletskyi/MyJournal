using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Subject
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Name { get; set; }
    }
}
