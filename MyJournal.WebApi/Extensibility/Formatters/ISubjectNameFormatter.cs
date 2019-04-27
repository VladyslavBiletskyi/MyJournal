using MyJournal.Domain.Entities;

namespace MyJournal.WebApi.Extensibility.Formatters
{
    public interface ISubjectNameFormatter
    {
        string Format(Subject group);
    }
}
