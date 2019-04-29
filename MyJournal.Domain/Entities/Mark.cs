using System;

namespace MyJournal.Domain.Entities
{
    public class Mark : BaseInstance
    {
        public LessonSkip LessonSkip { get; set; }

        public int Grade { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}