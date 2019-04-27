using MyJournal.Domain.Entities;
using MyJournal.WebApi.Extensibility.Formatters;

namespace MyJournal.WebApi.Formatters
{
    public class GroupNameFormatter : IGroupNameFormatter
    {
        public string Format(Group group)
        {
            return group.Letter != null ? $"{group.Year}-{group.Letter}" : group.Year.ToString();
        }
    }
}
