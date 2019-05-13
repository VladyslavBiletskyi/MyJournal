using System;

namespace MyJournal.Domain.Entities
{
    public class Lesson : BaseInstance
    {
        public Group Group { get; set; }

        public Subject Subject { get; set; }

        public Teacher Teacher { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsForThematicMarks { get; set; }

        public bool IsForSemesterMarks { get; set; }
    }
}