using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;

namespace MyJournal.Services.Services
{
    internal class SubjectService : ISubjectService
    {
        private ISubjectRepository subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
        }

        public IEnumerable<Subject> GetAll()
        {
            return subjectRepository.Instances().ToList();
        }

        public Subject Get(int id)
        {
            return subjectRepository.Find(id);
        }

        public bool Create(string name)
        {
            var existing = subjectRepository.Instances().FirstOrDefault(instance => instance.Name == name.ToUpperInvariant());
            if (existing != null)
            {
                return false;
            }
            var subject = new Subject { Name = name.ToUpperInvariant() };
            return subjectRepository.CreateInstance(subject);
        }
    }
}