using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Group
{
    public class AssignTeacherModel
    {
        [Required(ErrorMessage = "Клас повинен мати класного керівника")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Клас не вибрано")]
        public int GroupId { get; set; }
    }
}
