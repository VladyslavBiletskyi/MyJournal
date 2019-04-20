using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Domain.Repositories;
using MyJournal.Services.Extensibility;
using MyJournal.Services.Extensibility.Seeders;
using MyJournal.Services.Validation;

namespace MyJournal.Services
{
    internal class UserManager: IUserManager
    {
        private IInitialUserSeeder initialUserSeeder;

        private ITeacherRepository teacherRepository;

        private IStudentRepository studentRepository;

        private IGroupRepository groupRepository;

        private IPasswordHasher passwordHasher;

        public UserManager(IInitialUserSeeder initialUserSeeder, ITeacherRepository teacherRepository, IStudentRepository studentRepository, IPasswordHasher passwordHasher, IGroupRepository groupRepository)
        {
            this.initialUserSeeder = initialUserSeeder;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.passwordHasher = passwordHasher;
            this.groupRepository = groupRepository;
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

        public ValidationResult<ApplicationUser> Create(string login, string password, string firstName, string lastName, string surname, int groupId, bool isTeacher)
        {
            if (TryAuthenticate(login, password, out bool isUserFound) != null || isUserFound)
            {
                return new ValidationResult<ApplicationUser>("Пользователь с таким логином уже существует");
            }
            if (isTeacher)
            {
                Teacher teacher = CreateUser<Teacher>(login, password, firstName, lastName, surname, groupId);
                if (teacherRepository.CreateInstance(teacher))
                {
                    return new ValidationResult<ApplicationUser>(teacher);
                }
            }
            else
            {
                Student student = CreateUser<Student>(login, password, firstName, lastName, surname, groupId);
                if (student.Group == null)
                {
                    return new ValidationResult<ApplicationUser>("Ученик обязан относиться к существующему классу");
                }

                if (studentRepository.CreateInstance(student))
                {
                    return new ValidationResult<ApplicationUser>(student);
                }
            }
            return new ValidationResult<ApplicationUser>("Ошибка при регистрации пользователя");
        }

        private TUser CreateUser<TUser>(string login, string password, string firstName, string lastName, string surname, int groupId) where TUser : ApplicationUser, new()
        {
            var passwordData = passwordHasher.GetHash(password);
            var group = groupRepository.Find(groupId);
            return new TUser
            {
                Login = login,
                FirstName = firstName,
                LastName = lastName,
                Surname = surname,
                PasswordHash = passwordData.Item1,
                PasswordSalt = passwordData.Item2,
                Group = group
            };
        }
    }
}
