using System;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class TeacherRepository : ApplicationUserRepositoryBase<Teacher>, ITeacherRepository
    {
        public TeacherRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Teacher instance)
        {
            throw new NotImplementedException();
        }
    }
}
