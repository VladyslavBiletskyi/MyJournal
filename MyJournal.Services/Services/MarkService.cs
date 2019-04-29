using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Services
{
    internal class MarkService : IMarkService
    {
        private IMarkRepository markRepository;
        private IAttendRepository attendRepository;

        public MarkService(IMarkRepository markRepository, IAttendRepository attendRepository)
        {
            this.markRepository = markRepository;
            this.attendRepository = attendRepository;
        }

        public ValidationResult InsertBatch(IEnumerable<Mark> marks)
        {
            var validationResults = new List<string>();
            marks = marks.ToList();
            if (!attendRepository.BatchInsert(marks.Select(x => x.Attend).Where(x => x != null)))
            {
                validationResults.Add("Помилка при реєстрації пропусків");
            }
            if (!markRepository.BatchInsert(marks.Where(mark => mark.Grade >=0)))
            {
                validationResults.Add("Помилка при виставленні оцінок");
            }

            return new ValidationResult(validationResults.ToArray());
        }
    }
}
