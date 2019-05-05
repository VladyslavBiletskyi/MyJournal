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
        private ILessonRepository lessonRepository;

        public SubjectService(ISubjectRepository subjectRepository, ILessonRepository lessonRepository)
        {
            this.subjectRepository = subjectRepository;
            this.lessonRepository = lessonRepository;
        }

        public IEnumerable<Subject> GetAll()
        {
            return subjectRepository.Instances().OrderBy(x => x.Name).ToList();
        }

        public IEnumerable<Subject> GetSubjectsOfGroup(Group group)
        {
            return lessonRepository.Instances().Where(x => x.Group == group).Select(x => x.Subject).Distinct().OrderByDescending(x => x.Name).ToList();
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