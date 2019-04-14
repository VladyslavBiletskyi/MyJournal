using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility;

namespace MyJournal.Services
{
    internal class InitialUserSeeder : IInitialUserSeeder
    {
        private ITeacherRepository teacherRepository;
        private IPasswordHasher passwordHasher;

        public InitialUserSeeder(ITeacherRepository teacherRepository, IPasswordHasher passwordHasher)
        {
            this.teacherRepository = teacherRepository;
            this.passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (!teacherRepository.Instances().Any())
            {
                var passwordData = passwordHasher.GetHash("BVVi181296");
                var user = new Teacher()
                {
                    FirstName = "Владислав",
                    LastName = "Валерьевич",
                    Surname = "Билецкий",
                    Login = "vladyslav.biletskyi",
                    PasswordHash = passwordData.Item1,
                    PasswordSalt = passwordData.Item2
                };
                teacherRepository.CreateInstance(user);
            }
        }
    }
}
