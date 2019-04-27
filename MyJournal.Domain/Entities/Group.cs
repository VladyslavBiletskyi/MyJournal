using System.Collections.Generic;

namespace MyJournal.Domain.Entities
{
    public class Group : BaseInstance
    {
        public int Year { get; set; }

        public string Letter { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}