﻿namespace MyJournal.WebApi.Models.Mark
{
    public class DisplayMarkModel : LessonMarkModelBase
    {
        public string LessonName { get; set; }

        public bool IsThematic { get; set; }

        public bool IsSemester { get; set; }

        public bool IsYear { get; set; }
    }
}
