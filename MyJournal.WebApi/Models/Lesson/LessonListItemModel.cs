﻿namespace MyJournal.WebApi.Models.Lesson
{
    public class LessonListItemModel
    {
        public int LessonId { get; set; }

        public string SubjectName { get; set; }

        public bool IsForThematicMarks { get; set; }

        public bool IsForSemesterMarks { get; set; }

        public bool IsForYearMarks { get; set; }
    }
}
