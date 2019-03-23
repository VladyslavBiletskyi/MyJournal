using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Data
{
    public class MyJournalDbContext : IdentityDbContext, IDatabaseContext
    {
        public MyJournalDbContext(DbContextOptions<MyJournalDbContext> options)
            : base(options)
        {
        }

        public IQueryable<TInstance> Instances<TInstance>() where TInstance : class
        {
            return Set<TInstance>();
        }

        public TInstance Find<TInstance>(Func<TInstance, bool> selector) where TInstance : class
        {
            return Set<TInstance>().FirstOrDefault(selector);
        }

        public bool CreateInstance<TInstance>(TInstance instance) where TInstance : class
        {
            try
            {
                Set<TInstance>().Add(instance);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryRemoveInstance<TInstance>(TInstance instance) where TInstance : class
        {
            try
            {
                Set<TInstance>().Remove(instance);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}