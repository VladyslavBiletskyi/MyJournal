using System.Collections.Generic;

namespace MyJournal.WebApi.Models.Group
{
    public class DisplayGroupModel
    {
        public string Name { get; set; }

        public IEnumerable<DisplayStudentModel> Students { get; set; }
    }
}