using System.Collections.Generic;
using MyJournal.Domain.Entities;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IBatchInsertRepository<TInstance> where TInstance : BaseInstance
    {
        bool BatchInsert(IEnumerable<TInstance> instances);
    }
}
