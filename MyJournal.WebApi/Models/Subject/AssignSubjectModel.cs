using System.Collections.Generic;

namespace MyJournal.WebApi.Models.Subject
{
    public class AssignSubjectModel
    {
        public int SubjectId { get; set; }

        public int SubjectName { get; set; }

        public IEnumerable<AssignTeacherModel> TeacherData { get; set; }
    }
}
