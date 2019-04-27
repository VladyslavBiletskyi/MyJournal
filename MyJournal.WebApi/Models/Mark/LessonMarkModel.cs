using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Mark
{
    public class LessonMarkModel
    {
        public int LessonId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        [Range(1,12)]
        public int? Mark { get; set; }

        public bool NotPresent { get; set; }
    }
}
