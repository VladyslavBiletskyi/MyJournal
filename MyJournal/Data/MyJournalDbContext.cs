using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
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
            IQueryable<TInstance> query = Set<TInstance>();
            foreach (var property in typeof(TInstance).GetProperties().Where(x => 
                x.GetGetMethod().ReturnType.BaseType == typeof(TInstance).BaseType
                || x.GetGetMethod().ReturnType.BaseType == typeof(IdentityUser)
                || x.GetGetMethod().IsVirtual))
            {
                query = query.Include(property.Name);
            }
            return query;
        }

        public TInstance Find<TInstance>(Func<TInstance, bool> selector) where TInstance : class
        {
            return Instances<TInstance>().FirstOrDefault(selector);
        }

        public bool CreateInstance<TInstance>(TInstance instance) where TInstance : class
        {
            try
            {
                Set<TInstance>().Add(instance);
                SaveChanges();
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
                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}