using System;

namespace MyJournal.Domain.Entities
{
    public class ApplicationUser : BaseInstance
    {
        public string Role { get; protected set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Surname { get; set; }

        public Group Group { get; set; }

        public DateTime? LastActivity { get; set; }
    }
}
