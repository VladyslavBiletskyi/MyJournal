using System.Collections.Generic;
using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IMarkRepository : IRepositoryBase<Mark>, IBatchInsertRepository<Mark>
    {
    }
}
