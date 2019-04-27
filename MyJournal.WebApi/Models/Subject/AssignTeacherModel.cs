using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Subject
{
    public class AssignTeacherModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}
