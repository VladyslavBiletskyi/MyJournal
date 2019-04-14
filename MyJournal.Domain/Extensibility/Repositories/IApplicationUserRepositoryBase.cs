using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IApplicationUserRepositoryBase<TUser> : IRepositoryBase<TUser>
    where TUser : ApplicationUser
    {
        TUser FindByLogin(string login);
    }
}
