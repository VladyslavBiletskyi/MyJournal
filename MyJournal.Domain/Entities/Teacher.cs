using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyJournal.Domain.Entities
{
    public class Teacher : IdentityUser
    {
        public virtual ICollection<Subject> Subjects { get; set; }

        public Group Group { get; set; }
    }
}