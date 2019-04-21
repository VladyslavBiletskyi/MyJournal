using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyJournal.WebApi.Models.Subject
{
    public class AssignTeacherModel
    {
        public int TeacherId { get; set; }

        public string TeacherName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
