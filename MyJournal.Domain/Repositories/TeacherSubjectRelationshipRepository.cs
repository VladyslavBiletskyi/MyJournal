using System;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    internal class TeacherSubjectRelationshipRepository : RepositoryBase<TeacherSubjectRelation>, ITeacherSubjectRelationshipRepository
    {
        public TeacherSubjectRelationshipRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(TeacherSubjectRelation instance)
        {
            throw new NotImplementedException();
        }
    }
}
