using System.Collections.Generic;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class LessonSkipRepository : RepositoryBase<LessonSkip>, ILessonSkipRepository
    {
        public LessonSkipRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public bool BatchInsert(IEnumerable<LessonSkip> instances)
        {
            return DatabaseContext.CreateInstances(instances);
        }

        public override bool TryUpdateInstance(LessonSkip instance)
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
