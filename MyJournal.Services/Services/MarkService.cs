using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IUserNameFormatter userNameFormatter;
        private ILessonRepository lessonRepository;

        public MarkService(IMarkRepository markRepository, ILessonSkipRepository lessonSkipRepository, IDateTimeFormatter dateTimeFormatter, ILessonRepository lessonRepository, IUserNameFormatter userNameFormatter)
        {
            this.markRepository = markRepository;
            this.lessonSkipRepository = lessonSkipRepository;
            this.dateTimeFormatter = dateTimeFormatter;
            this.lessonRepository = lessonRepository;
            this.userNameFormatter = userNameFormatter;
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

        public string Export(Subject subject, Group group)
        {
            var marksForSemester = markRepository.Instances()
                .Where(x => x.Lesson.Group == group && x.Lesson.Subject == subject).GroupBy(x => x.Lesson)
                .OrderBy(x => x.Key.DateTime).ToList();

            var lessonDatesForSemester = lessonRepository.Instances().Where(x => x.Subject == subject && x.Group == group).Select(x => x.DateTime).ToList();

            var studentsMarksByStudents = new Dictionary<Student, List<Tuple<DateTime, string>>>();
            foreach (var student in group.Students)
            {
                studentsMarksByStudents[student] = new List<Tuple<DateTime, string>>();
            }
            foreach (var markGroup in marksForSemester)
            {
                foreach (var mark in markGroup)
                {
                    if (studentsMarksByStudents.ContainsKey(mark.Student))
                    {
                        studentsMarksByStudents[mark.Student].Add(new Tuple<DateTime, string>(mark.Lesson.DateTime.Date, mark.LessonSkip == null?
                            mark.IsThematic ? $"[Тематична] {mark.Grade.ToString()}": mark.Grade.ToString()
                            : "Н"));
                    }
                }
            }

            var extendedStudentsMarksByStudents = new Dictionary<Student, List<Tuple<DateTime, string>>>();

            foreach (var marksOfStudent in studentsMarksByStudents)
            {
                var emptyLessonDates = lessonDatesForSemester.Where(lessonDate => marksOfStudent.Value.All(value => value.Item1 != lessonDate.Date));
                extendedStudentsMarksByStudents[marksOfStudent.Key] = studentsMarksByStudents[marksOfStudent.Key]
                    .Concat(emptyLessonDates.Select(x => new Tuple<DateTime, string>(x, String.Empty)))
                    .OrderBy(x => x.Item1).ToList();
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("П.І.Б. учня;");
            if (extendedStudentsMarksByStudents.Any())
            {
                var exportDelimiter = ";";
                stringBuilder.Append(String.Join(exportDelimiter, extendedStudentsMarksByStudents.FirstOrDefault().Value.Select(x => dateTimeFormatter.FormatWithoutTime(x.Item1))));
                
                foreach (var extendedMarksOfStudent in extendedStudentsMarksByStudents)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append(userNameFormatter.FormatFull(extendedMarksOfStudent.Key) + exportDelimiter);
                    stringBuilder.Append(String.Join(exportDelimiter, extendedMarksOfStudent.Value.Select(x => x.Item2)));
                }
            }

            return stringBuilder.ToString();
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
