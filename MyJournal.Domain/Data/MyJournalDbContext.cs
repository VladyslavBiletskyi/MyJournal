using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Data
{
    public class MyJournalDbContext : DbContext, IDatabaseContext
    {
        public MyJournalDbContext(DbContextOptions<MyJournalDbContext> options) : base(options)
        {
        }

        public DbSet<Attend> Attends { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        public IQueryable<TInstance> Instances<TInstance>() where TInstance : class
        {
            IQueryable<TInstance> query = Set<TInstance>();
            foreach (var property in typeof(TInstance).GetProperties().Where(x =>
                x.GetGetMethod().ReturnType.BaseType == typeof(TInstance).BaseType
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
