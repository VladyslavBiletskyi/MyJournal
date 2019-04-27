using MyJournal.Domain.Entities;
using MyJournal.WebApi.Extensibility.Formatters;

namespace MyJournal.WebApi.Formatters
{
    public class SubjectNameFormatter : ISubjectNameFormatter
    {
        public string Format(Subject subject)
        {
            var originalLower = subject.Name.ToLowerInvariant();
            return originalLower[0].ToString().ToUpperInvariant() + originalLower.Substring(1);
        }
    }
}