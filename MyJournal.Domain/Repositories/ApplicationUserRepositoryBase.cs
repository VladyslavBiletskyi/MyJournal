using System.Linq;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class ApplicationUserRepositoryBase<TUser> : RepositoryBase<TUser>, IApplicationUserRepositoryBase<TUser> where TUser : ApplicationUser
    {
        public ApplicationUserRepositoryBase(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }

        public TUser FindByLogin(string login)
        {
            return Instances().FirstOrDefault(instance => instance.Login == login);
        }

        public override bool TryUpdateInstance(TUser instance)
        {
            throw new System.NotImplementedException();
        }
    }
}
