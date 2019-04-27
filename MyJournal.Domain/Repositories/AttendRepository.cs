using System.Collections.Generic;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class AttendRepository : RepositoryBase<Attend>, IAttendRepository
    {
        public AttendRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public bool BatchInsert(IEnumerable<Attend> instances)
        {
            return DatabaseContext.CreateInstances(instances);
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
