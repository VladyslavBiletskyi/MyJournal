using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public class LessonRepository : RepositoryBase<Lesson>, ILessonRepository
    {
        public LessonRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Lesson instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Group = instance.Group;
            original.DateTime = instance.DateTime;
            original.Subject = instance.Subject;
            original.Teacher = instance.Teacher;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
