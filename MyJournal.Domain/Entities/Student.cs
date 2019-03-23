using Microsoft.AspNetCore.Identity;

namespace MyJournal.Domain.Entities
{
    public class Student : IdentityUser
    {
        public Group Group { get; set; }
    }
}