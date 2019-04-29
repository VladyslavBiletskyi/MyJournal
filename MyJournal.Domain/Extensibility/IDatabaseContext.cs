using System;
using System.Collections.Generic;
using System.Linq;

namespace MyJournal.Domain.Extensibility
{
    public interface IDatabaseContext
    {
        IQueryable<TInstance> Instances<TInstance>() where TInstance : class;

        TInstance Find<TInstance>(Func<TInstance,bool> selector) where TInstance : class;

        bool CreateInstance<TInstance>(TInstance instance) where TInstance : class;

        bool CreateInstances<TInstance>(IEnumerable<TInstance> instances) where TInstance : class;

        bool TryRemoveInstance<TInstance>(TInstance instance) where TInstance : class;

        int SaveChanges();
    }
}