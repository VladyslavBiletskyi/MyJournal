﻿using System.Collections.Generic;

namespace MyJournal.Domain.Entities
{
    public class Teacher : ApplicationUser
    {
        public Teacher()
        {
            Role = nameof(Teacher);
        }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}