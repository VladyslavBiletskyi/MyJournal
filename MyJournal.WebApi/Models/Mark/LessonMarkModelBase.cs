using System.ComponentModel.DataAnnotations;

namespace MyJournal.WebApi.Models.Mark
{
    public abstract class LessonMarkModelBase
    {
        [Range(1, 12)]
        public int? Mark { get; set; }

        public bool NotPresent { get; set; }
    }
}
