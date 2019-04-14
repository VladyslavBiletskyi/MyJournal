using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface ITeacherRepository : IRepositoryBase<Teacher>, IApplicationUserRepositoryBase<Teacher>
    {
    }
}
