using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Group instance)
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
