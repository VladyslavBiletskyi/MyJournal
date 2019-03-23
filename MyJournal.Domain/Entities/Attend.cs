namespace MyJournal.Domain.Entities
{
    public class Attend : BaseInstance
    {
        public Student Student { get; set; }

        public Lesson Lesson { get; set; }
    }
}