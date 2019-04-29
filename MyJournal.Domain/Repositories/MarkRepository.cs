using System;
using System.Collections.Generic;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class MarkRepository : RepositoryBase<Mark>, IMarkRepository
    {
        public MarkRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public bool BatchInsert(IEnumerable<Mark> marks)
        {
            return DatabaseContext.CreateInstances(marks);
        }

        public override bool TryUpdateInstance(Mark instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.LessonSkip = instance.LessonSkip;
            original.Grade = instance.Grade;
            original.UpdateTime = DateTime.Now;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
