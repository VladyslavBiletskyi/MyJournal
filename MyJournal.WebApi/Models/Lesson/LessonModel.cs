using System.Collections.Generic;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Models.Lesson
{
    public class LessonModel
    {
        public int LessonId { get; set; }

        public string SubjectName { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<LessonMarkModel> MarksData { get; set; }
    }
}
