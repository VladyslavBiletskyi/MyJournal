using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public class AttendRepository : RepositoryBase<Attend>, IAttendRepository
    {
        public AttendRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Attend instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Lesson = instance.Lesson;
            original.Student = instance.Student;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
