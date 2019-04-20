using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Group instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Year = instance.Year;
            original.Letter = instance.Letter;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
