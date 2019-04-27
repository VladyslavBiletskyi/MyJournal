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
            if (instance == null)
            {
                return false;
            }
            var originalInstance = Find(instance.Id);
            if (originalInstance != null)
            {
                originalInstance.FirstName = instance.FirstName;
                originalInstance.LastName = instance.LastName;
                originalInstance.Surname = instance.Surname;
                originalInstance.PasswordSalt = instance.PasswordSalt;
                originalInstance.PasswordHash = instance.PasswordHash;
                originalInstance.Group = instance.Group;
                DatabaseContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
