using System.Collections.Generic;
using MyJournal.WebApi.Models.Subject;

namespace MyJournal.WebApi.Models.Mark
{
    public class SubjectFilteredTimeSpanModel : TimeSpanModel
    {
        public int SubjectId { get; set; }

        public IEnumerable<SubjectModel> SubjectsForSelection { get; set; }
    }
}
