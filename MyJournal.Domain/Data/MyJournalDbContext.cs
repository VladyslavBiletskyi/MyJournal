using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Data
{
    public class MyJournalDbContext : DbContext, IDatabaseContext
    {
        public MyJournalDbContext(DbContextOptions<MyJournalDbContext> options) : base(options)
        {
        }

        public DbSet<LessonSkip> LessonSkips { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<TeacherSubjectRelation> TeacherSubjectRelations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherSubjectRelation>()
                .HasOne(x => x.Teacher)
                .WithMany(x => x.SubjectRelations)
                .HasForeignKey(x => x.TeacherId);

            modelBuilder.Entity<TeacherSubjectRelation>()
                .HasOne(x => x.Subject)
                .WithMany()
                .HasForeignKey(x => x.SubjectId);

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<TInstance> Instances<TInstance>() where TInstance : class
        {
            IQueryable<TInstance> query = Include(Set<TInstance>(), GetIncludePaths(typeof(TInstance)));
            
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

        public bool CreateInstances<TInstance>(IEnumerable<TInstance> instances) where TInstance : class
        {
            try
            {
                foreach (var instance in instances)
                {
                    Set<TInstance>().Add(instance);
                }

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

        private IQueryable<TInstance> Include<TInstance>(IQueryable<TInstance> source, IEnumerable<string> navigationPropertyPaths)
            where TInstance : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        private IEnumerable<string> GetIncludePaths(Type clrEntityType)
        {
            var entityType = Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                var entityNavigations = new List<INavigation>();
                foreach (var navigation in entityType.GetNavigations())
                {
                    if (includedNavigations.Add(navigation))
                        entityNavigations.Add(navigation);
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.FindInverse();
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.GetTargetType();
            }
        }

    }
}
