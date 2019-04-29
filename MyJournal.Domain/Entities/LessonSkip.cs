namespace MyJournal.Domain.Entities
{
    public class LessonSkip : BaseInstance
    {
        public Student Student { get; set; }

        public Lesson Lesson { get; set; }
    }
}