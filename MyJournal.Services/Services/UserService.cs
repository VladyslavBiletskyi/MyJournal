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

        public UserService(ITeacherRepository teacherRepository, IStudentRepository studentRepository)
        {
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
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

        public ApplicationUser FindUser(int id)
        {
            ApplicationUser teacher = teacherRepository.Find(id);
            return teacher ?? studentRepository.Find(id);
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
                return teacherRepository.TryUpdateInstance(teacher);
            }
            else
            {
                return studentRepository.TryUpdateInstance(user as Student);
            }
        }
    }
}
