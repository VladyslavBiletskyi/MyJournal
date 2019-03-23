using System.Collections.Generic;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public abstract class RepositoryBase<TInstance>: IRepositoryBase<TInstance> where TInstance : BaseInstance
    {
        private readonly IDatabaseContext databaseContext;

        protected RepositoryBase(IDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public IEnumerable<TInstance> Instances()
        {
            return databaseContext.Instances<TInstance>();
        }

        public TInstance Find(int key)
        {
            return databaseContext.Find((TInstance instance) => instance.Id == key);
        }

        public bool CreateInstance(TInstance instance)
        {
            return databaseContext.CreateInstance(instance);
        }

        public bool TryRemoveInstance(TInstance instance)
        {
            var originalInstance = Find(instance.Id);
            return originalInstance != null && databaseContext.TryRemoveInstance(originalInstance);
        }

        public abstract bool TryUpdateInstance(TInstance instance);
    }
}