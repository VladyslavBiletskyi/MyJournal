using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Domain.Extensibility
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