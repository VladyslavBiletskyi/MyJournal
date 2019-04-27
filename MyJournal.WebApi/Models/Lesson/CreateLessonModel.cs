using System;

namespace MyJournal.WebApi.Models.Lesson
{
    public class CreateLessonModel
    {
        public int SubjectId { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
