namespace MyJournal.Domain.Entities
{
    public class Student: ApplicationUser
    {
        public Student()
        {
            Role = nameof(Student);
        }
    }
}