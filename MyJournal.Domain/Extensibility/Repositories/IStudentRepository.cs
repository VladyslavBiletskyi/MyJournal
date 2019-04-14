using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IStudentRepository : IRepositoryBase<Student>, IApplicationUserRepositoryBase<Student>
    {
    }
}
