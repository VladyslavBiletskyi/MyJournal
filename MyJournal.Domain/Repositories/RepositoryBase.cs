using System.Collections.Generic;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public abstract class RepositoryBase<TInstance>: IRepositoryBase<TInstance> where TInstance : BaseInstance
    {
        protected readonly IDatabaseContext DatabaseContext;

        protected RepositoryBase(IDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public IEnumerable<TInstance> Instances()
        {
            return DatabaseContext.Instances<TInstance>();
        }

        public TInstance Find(int key)
        {
            return DatabaseContext.Find((TInstance instance) => instance.Id == key);
        }

        public virtual bool CreateInstance(TInstance instance)
        {
            return DatabaseContext.CreateInstance(instance);
        }

        public virtual bool TryRemoveInstance(TInstance instance)
        {
            var originalInstance = Find(instance.Id);
            return originalInstance != null && DatabaseContext.TryRemoveInstance(originalInstance);
        }

        public abstract bool TryUpdateInstance(TInstance instance);
    }
}