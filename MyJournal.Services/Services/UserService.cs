using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;

namespace MyJournal.Services.Services
{
    internal class UserService : IUserService
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherSubjectRelationshipRepository teacherSubjectRelationshipRepository;

        public UserService(ITeacherRepository teacherRepository, IStudentRepository studentRepository, ITeacherSubjectRelationshipRepository teacherSubjectRelationshipRepository)
        {
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.teacherSubjectRelationshipRepository = teacherSubjectRelationshipRepository;
        }

        public IEnumerable<ApplicationUser> GetTeachers()
        {
            return teacherRepository.Instances().ToList();
        }

        public IEnumerable<ApplicationUser> GetStudents()
        {
            return studentRepository.Instances().ToList();
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return GetTeachers().Concat(GetStudents());
        }

        public Teacher FindTeacher(int id)
        {
            return teacherRepository.Find(id);
        }

        public Student FindStudent(int id)
        {
            return studentRepository.Find(id);
        }

        public ApplicationUser FindUser(int id)
        {
            ApplicationUser teacher = FindTeacher(id);
            return teacher ?? FindStudent(id);
        }

        public ApplicationUser FindUser(string login)
        {
            if (login == null)
            {
                return null;
            }
            ApplicationUser teacher = teacherRepository.FindByLogin(login);
            return teacher ?? studentRepository.FindByLogin(login);
        }

        public bool Update(ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }

            var teacher = user as Teacher;
            if (teacher != null)
            {
                var original = teacherRepository.Find(teacher.Id);
                var newSubjectRelations = teacher.SubjectRelations.Except(original.SubjectRelations).ToList();
                var isSucceeded = teacherRepository.TryUpdateInstance(teacher);
                if (isSucceeded && newSubjectRelations.Any())
                {
                    foreach (var teacherSubjectRelation in newSubjectRelations)
                    {
                        teacherSubjectRelationshipRepository.CreateInstance(teacherSubjectRelation);
                    }
                }

                return isSucceeded;
            }
            else
            {
                return studentRepository.TryUpdateInstance(user as Student);
            }
        }
    }
}
