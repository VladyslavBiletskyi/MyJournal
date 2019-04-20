using System.Collections.Generic;
using System.Linq;

namespace MyJournal.Services.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            ValidationMessages = new List<string>();
        }

        public ValidationResult(params string[] messages)
        {
            ValidationMessages = messages;
        }

        public IEnumerable<string> ValidationMessages { get; set; }

        public bool IsValid => !ValidationMessages.Any();
    }

    public class ValidationResult<TData> : ValidationResult where TData : class
    {
        public ValidationResult(params string[] messages) : base(messages)
        {
        }

        public ValidationResult(TData data, params string[] messages) : base(messages)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}