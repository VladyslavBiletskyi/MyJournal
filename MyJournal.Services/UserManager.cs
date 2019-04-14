using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Domain.Repositories;
using MyJournal.Services.Extensibility;

namespace MyJournal.Services
{
    internal class UserManager: IUserManager
    {
        private IInitialUserSeeder initialUserSeeder;

        private ITeacherRepository teacherRepository;

        private IStudentRepository studentRepository;

        private IPasswordHasher passwordHasher;

        public UserManager(IInitialUserSeeder initialUserSeeder, ITeacherRepository teacherRepository, IStudentRepository studentRepository, IPasswordHasher passwordHasher)
        {
            this.initialUserSeeder = initialUserSeeder;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.passwordHasher = passwordHasher;
        }

        public ApplicationUser TryAuthenticate(string login, string password, out bool isUserFound)
        {
            initialUserSeeder.Seed();
            ApplicationUser user = teacherRepository.FindByLogin(login);
            if (user == null)
            {
                user = studentRepository.FindByLogin(login);
                if (user == null)
                {
                    isUserFound = false;
                    return null;
                }
            }

            isUserFound = true;
            if (passwordHasher.CheckPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return user;
            }

            return null;
        }
    }
}
