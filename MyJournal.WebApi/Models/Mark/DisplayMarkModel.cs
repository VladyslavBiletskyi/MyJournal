namespace MyJournal.WebApi.Models.Mark
{
    public class DisplayMarkModel : LessonMarkModelBase
    {
        public string LessonName { get; set; }

        public bool IsThematic { get; set; }
    }
}
