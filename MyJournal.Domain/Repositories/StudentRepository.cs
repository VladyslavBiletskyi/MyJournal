using System;
using MyJournal.Domain.Data;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;

namespace MyJournal.Domain.Repositories
{
    public class StudentRepository: ApplicationUserRepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(MyJournalDbContext databaseContext) : base(databaseContext)
        {
        }
    }
}
