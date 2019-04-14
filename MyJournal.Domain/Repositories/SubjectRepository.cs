using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Subject instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Name = instance.Name;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
