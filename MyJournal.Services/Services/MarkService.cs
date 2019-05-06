using System;
using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Formatters;
using MyJournal.Services.Extensibility.Services;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Services
{
    internal class MarkService : IMarkService
    {
        private IMarkRepository markRepository;
        private ILessonSkipRepository lessonSkipRepository;
        private IDateTimeFormatter dateTimeFormatter;

        public MarkService(IMarkRepository markRepository, ILessonSkipRepository lessonSkipRepository, IDateTimeFormatter dateTimeFormatter)
        {
            this.markRepository = markRepository;
            this.lessonSkipRepository = lessonSkipRepository;
            this.dateTimeFormatter = dateTimeFormatter;
        }

        public IEnumerable<Mark> GetMarksOfLesson(Lesson lesson)
        {
            return markRepository.Instances().Where(x => x.Lesson == lesson).ToList()
                .Concat(lessonSkipRepository.Instances()
                .Where(x => x.Lesson == lesson).Select(LessonSkipToMark));
        }

        public IDictionary<DateTime, IEnumerable<Mark>> GetMarksWithSkips(Student student, DateTime fromDay, DateTime toDay)
        {
            var userMarks = markRepository.Instances().Where(x => x.Student == student && x.Lesson.DateTime >= fromDay && x.Lesson.DateTime < toDay).ToList();
            var userSkips = lessonSkipRepository.Instances().Where(x => x.Student == student && x.Lesson.DateTime >= fromDay && x.Lesson.DateTime < toDay).ToList();
            return OrderAndGroupMarks(userMarks.Concat(userSkips.Select(LessonSkipToMark)));
        }

        public IDictionary<DateTime, IEnumerable<Mark>> GetMarks(Student student, DateTime fromDay, DateTime toDay)
        {
            var userMarks = markRepository.Instances().Where(x => x.Student == student && x.Lesson.DateTime >= fromDay && x.Lesson.DateTime < toDay).ToList();
            return OrderAndGroupMarks(userMarks);
        }

        public ValidationResult InsertBatch(IEnumerable<Mark> marks)
        {
            var validationResults = new List<string>();
            marks = marks.ToList();
            if (!lessonSkipRepository.BatchInsert(marks.Select(x => x.LessonSkip).Where(x => x != null)))
            {
                validationResults.Add("Помилка при реєстрації пропусків");
            }
            if (!markRepository.BatchInsert(marks.Where(mark => mark.Grade != null)))
            {
                validationResults.Add("Помилка при виставленні оцінок");
            }

            return new ValidationResult(validationResults.ToArray());
        }

        public void Export(Subject subject, Group group)
        {
            var marksForSemester = markRepository.Instances()
                .Where(x => x.Lesson.Group == group && x.Lesson.Subject == subject).GroupBy(x => x.Lesson)
                .OrderBy(x => x.Key.DateTime);
            var columnNames = new[] { "П.І.Б. учня" }.Concat(marksForSemester.Select(x => dateTimeFormatter.FormatWithoutTime(x.Key.DateTime)));
            var rows = new List<string>();

            
        }

        private IDictionary<DateTime, IEnumerable<Mark>> OrderAndGroupMarks(IEnumerable<Mark> marks)
        {
            return marks.GroupBy(x => x.Lesson.DateTime.Date, x => x).OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Select(value => value));
        }

        private Mark LessonSkipToMark(LessonSkip skip)
        {
            return new Mark
            {
                Lesson = skip.Lesson,
                Student = skip.Student,
                LessonSkip = skip
            };
        }

        private TimeSpan GetTimeFromSemesterStart()
        {
            if (DateTime.Today.Month >= 9)
            {
                return DateTime.Now - new DateTime(DateTime.Today.Year, 9, 1);
            }
            else
            {
                return DateTime.Now - new DateTime(DateTime.Today.Year, 1, 1);
            }
        }
    }
}
