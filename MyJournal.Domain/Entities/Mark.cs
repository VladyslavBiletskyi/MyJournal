﻿using System;

namespace MyJournal.Domain.Entities
{
    public class Mark : BaseInstance
    {
        public LessonSkip LessonSkip { get; set; }

        public int? Grade { get; set; }

        public Student Student { get; set; }

        public Lesson Lesson { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsThematic { get; set; }

        public bool IsSemester { get; set; }

        public bool IsYear { get; set; }
    }
}