using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Mark
{
    public class LessonMarkModel : LessonMarkModelBase
    {
        public int LessonId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }
    }
}
