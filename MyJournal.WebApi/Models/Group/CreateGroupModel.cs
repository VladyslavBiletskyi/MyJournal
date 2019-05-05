using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Group
{
    public class CreateGroupModel
    {
        [Required(ErrorMessage = "Клас повинен мати цифру")]
        public int Year { get; set; }

        public string Letter { get; set; }
    }
}
