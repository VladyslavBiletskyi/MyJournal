using MyJournal.Domain.Entities;

namespace MyJournal.WebApi.Extensibility.Formatters
{
    public interface IGroupNameFormatter
    {
        string Format(Group group);
    }
}
