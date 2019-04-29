using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IAttendRepository : IRepositoryBase<Attend>, IBatchInsertRepository<Attend>
    {
    }
}
