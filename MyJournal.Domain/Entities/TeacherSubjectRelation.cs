namespace MyJournal.Domain.Entities
{
    public class TeacherSubjectRelation : BaseInstance
    {
        public int TeacherId { get; set; }

        public int SubjectId { get; set; }

        public Teacher Teacher { get; set; }

        public Subject Subject { get; set; }
    }
}
