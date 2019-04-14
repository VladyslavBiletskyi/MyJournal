using System.Collections.Generic;

namespace MyJournal.Domain.Extensibility.Repositories
{
    public interface IRepositoryBase<TInstance> where TInstance : class
    {
        IEnumerable<TInstance> Instances();

        TInstance Find(int key);

        bool CreateInstance(TInstance instance);

        bool TryRemoveInstance(TInstance instance);

        bool TryUpdateInstance(TInstance instance);
    }
}