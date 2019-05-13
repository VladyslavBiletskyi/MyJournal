using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Models.Mark;
using MyJournal.WebApi.Models.Subject;

namespace MyJournal.WebApi.Controllers
{
    public class MarkController : Controller
    {
        private readonly IMarkService markService;
        private readonly IUserService userService;
        private readonly ILessonService lessonService;
        private readonly ICurrentUserProvider currentUserProvider;
        private readonly ISubjectNameFormatter subjectNameFormatter;
        private readonly ISubjectService subjectService;

        public MarkController(IMarkService markService, IUserService userService, ILessonService lessonService, ICurrentUserProvider currentUserProvider, ISubjectNameFormatter subjectNameFormatter, ISubjectService subjectService)
        {
            this.markService = markService;
            this.userService = userService;
            this.lessonService = lessonService;
            this.currentUserProvider = currentUserProvider;
            this.subjectNameFormatter = subjectNameFormatter;
            this.subjectService = subjectService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult MarksForTimeSpan()
        {
            var model = new TimeSpanModel()
            {
                DateFrom = GetDateOfNearestMonday(),
                DateTo = DateTime.Today
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult MarksForTimeSpan(TimeSpanModel model)
        {
            var user = currentUserProvider.GetCurrentUser<Student>(User);
            if (user == null)
            {
                return View(model);
            }

            return GetMarksForTimeSpan(user, model.DateFrom, model.DateTo);
        }

        [HttpGet]
        [Authorize]
        public IActionResult FilteredMarksForTimeSpan()
        {
            var user = currentUserProvider.GetCurrentUser<Student>(User);
            if (user == null)
            {
                return View();
            }

            var model = new SubjectFilteredTimeSpanModel
            {
                DateFrom = GetDateOfNearestMonday(),
                DateTo = DateTime.Today,
                SubjectsForSelection = subjectService.GetSubjectsOfGroup(user.Group).Select(x => new SubjectModel {SubjectId = x.Id, Name = subjectNameFormatter.Format(x)})
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult FilteredMarksForTimeSpan(SubjectFilteredTimeSpanModel model)
        {
            var user = currentUserProvider.GetCurrentUser<Student>(User);
            if (user == null)
            {
                return View(model);
            }

            return GetMarksForTimeSpan(user, model.DateFrom, model.DateTo, model.SubjectId);
        }

        [HttpGet]
        [Authorize]
        public IActionResult MarksForWeek()
        {
            var user = currentUserProvider.GetCurrentUser<Student>(User);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return GetMarksForTimeSpan(user, GetDateOfNearestMonday(), DateTime.Today);
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult InsertBatch([FromForm]IEnumerable<LessonMarkModel> model)
        {
            return InsertInternal(model, marks => markService.InsertBatch(marks));
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Export()
        {
            var teacher = currentUserProvider.GetCurrentUser<Teacher>(User);
            if (teacher?.Group == null)
            {
                return RedirectToAction("Index");
            }
            var subjectsOfGroup = subjectService.GetSubjectsOfGroup(teacher.Group).Select(x => new SubjectModel { SubjectId = x.Id, Name = subjectNameFormatter.Format(x) });
            return View(subjectsOfGroup);
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public async Task<IActionResult> Export(int subjectId)
        {
            var teacher = currentUserProvider.GetCurrentUser<Teacher>(User);
            if (teacher?.Group == null)
            {
                return View("ExportEmpty");
            }

            var subject = subjectService.Get(subjectId);

            var exportResult = markService.Export(subject, teacher.Group);
            var exportBytes = Encoding.GetEncoding("windows-1251").GetBytes(exportResult);

            var stream = new MemoryStream();
            await stream.WriteAsync(exportBytes, 0, exportBytes.Length);
            stream.Position = 0;
            return File(stream, "text/csv", subjectNameFormatter.Format(subject) + ".csv");
        }

        private IActionResult InsertInternal([FromForm]IEnumerable<LessonMarkModel> model, Action<IEnumerable<Mark>> action)
        {
            if (!model.Any() || !IsBatchValid(model))
            {
                return RedirectToAction("Index", "Lesson");
            }

            FixWrongPresenceData(model);
            var lesson = lessonService.Get(model.First().LessonId);

            var marksForSaving = model.Select(x =>
            {
                var student = userService.FindStudent(x.StudentId);
                return new Mark
                {
                    LessonSkip = x.NotPresent
                        ? new LessonSkip()
                        {
                            Student = student,
                            Lesson = lesson
                        }
                        : null,
                    Grade = x.Mark,
                    Lesson = lesson,
                    Student = student,
                    UpdateTime = DateTime.Now,
                    IsThematic = lesson.IsForThematicMarks,
                    IsSemester = lesson.IsForSemesterMarks
                };
            }).ToList();

            action(marksForSaving);
            return RedirectToAction("Index", "Lesson");
        }

        private IActionResult GetMarksForTimeSpan(Student user, DateTime dateFrom, DateTime dateTo, int? subjectId = null)
        {
            var markGroups = markService.GetMarksWithSkips(user, dateFrom, dateTo.AddDays(1));

            var convertedMarks = markGroups.ToDictionary(x => x.Key, x => x.Value.Where(y => !subjectId.HasValue || y.Lesson.Subject.Id == subjectId.Value).Select(value =>
                new DisplayMarkModel
                {
                    LessonName = subjectNameFormatter.Format(value.Lesson.Subject),
                    Mark = value.Grade,
                    NotPresent = value.LessonSkip != null,
                    IsThematic = value.IsThematic,
                    IsSemester = value.IsSemester
                }));
            return View("Display", convertedMarks);
        }

        private DateTime GetDateOfNearestMonday()
        {
            var daysFromMonday = (7 - ((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7) % 7;
            return DateTime.Today.AddDays(-daysFromMonday);
        }

        private void FixWrongPresenceData(IEnumerable<LessonMarkModel> model)
        {
            foreach (var markModel in model)
            {
                if (markModel.NotPresent && markModel.Mark != null)
                {
                    markModel.NotPresent = false;
                }
            }
        }

        private bool IsBatchValid(IEnumerable<LessonMarkModel> model)
        {
            var differentLessonsCount = model.GroupBy(x => x.LessonId).Count();
            return differentLessonsCount == 1;
        }
    }
}