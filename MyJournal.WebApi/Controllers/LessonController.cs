using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Attributes;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Models.Lesson;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    [UpdateActivity]
    public class LessonController : Controller
    {
        private readonly IUserService userService;
        private readonly ILessonService lessonService;
        private readonly IGroupNameFormatter groupNameFormatter;
        private readonly ISubjectNameFormatter subjectNameFormatter;
        private readonly ICurrentUserProvider currentUserProvider;
        private readonly IMarkService markService;

        public LessonController(
            IUserService userService, ILessonService lessonService, IGroupNameFormatter groupNameFormatter, ISubjectNameFormatter subjectNameFormatter, ICurrentUserProvider currentUserProvider, IMarkService markService)
        {
            this.userService = userService;
            this.lessonService = lessonService;
            this.groupNameFormatter = groupNameFormatter;
            this.subjectNameFormatter = subjectNameFormatter;
            this.currentUserProvider = currentUserProvider;
            this.markService = markService;
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Display(int lessonId)
        {
            return DisplayInternal(lessonId, "Display", true);
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Fill(int lessonId)
        {
            return DisplayInternal(lessonId, "Fill", false);
        }

        private IActionResult DisplayInternal(int lessonId, string viewName, bool shouldLoadValues)
        {
            var lesson = lessonService.Get(lessonId);
            if (lesson == null || lesson.Group == null)
            {
                return RedirectToAction("Index");
            }


            var marksByStudentId = shouldLoadValues? markService.GetMarksOfLesson(lesson).ToDictionary(x => x.Student.Id, x => x) : new Dictionary<int, Mark>();

            var markModels = lesson.Group.Students?.OrderBy(x => x.Surname).ThenBy(x => x.FirstName).Select(x => new LessonMarkModel
            {
                LessonId = lessonId,
                Mark = shouldLoadValues && marksByStudentId.ContainsKey(x.Id) ? marksByStudentId[x.Id].Grade : null,
                NotPresent = shouldLoadValues && marksByStudentId.ContainsKey(x.Id) && marksByStudentId[x.Id].LessonSkip != null,
                StudentId = x.Id,
                StudentName = $"{x.Surname} {x.FirstName} {x.LastName}"
            }) ?? new List<LessonMarkModel>();

            return View(viewName, new LessonModel
            {
                DateTime = lesson.DateTime,
                LessonId = lessonId,
                MarksData = markModels,
                GroupName = groupNameFormatter.Format(lesson.Group),
                SubjectName = subjectNameFormatter.Format(lesson.Subject),
                IsForThematicMarks = lesson.IsForThematicMarks,
                IsForSemesterMarks = lesson.IsForSemesterMarks,
                IsForYearMarks = lesson.IsForYearMarks
            });
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Create(CreateLessonModel model)
        {
            var teacher = currentUserProvider.GetCurrentUser<Teacher>(User);
            if (teacher == null)
            {
                return RedirectToAction("Index");
            }

            if (model.DateTime < DateTime.Today.AddYears(-1))
            {
                ModelState.AddModelError(nameof(model.DateTime), "Вказана недійсна дата.");
                return View();
            }

            if (model.GroupId == -1)
            {
                ModelState.AddModelError(nameof(model.GroupId), "Клас не вказаний.");
                return View();
            }

            if (!IsTeacherAssociatedForGroup(teacher, model.GroupId) && !IsTeacherAssociatedForSubject(teacher, model.SubjectId))
            {
                ModelState.AddModelError(nameof(model.TeacherId), "Тільки викладач, що може викладати даний предмет або є класним керівником класу, може ставити урок.");
                return View();
            }

            var lessonTeacher = userService.FindTeacher(model.TeacherId);
            if (lessonTeacher == null || !IsTeacherAssociatedForSubject(lessonTeacher, model.SubjectId))
            {
                ModelState.AddModelError(nameof(model.TeacherId), "Викладач не може викладати даний предмет.");
                return View();
            }

            if (!IsValidType(model))
            {
                ModelState.AddModelError(nameof(model.IsForSemesterMarks), "Урок не може одночасно бути для семестрових, річних та тематичних оцінок, будь ласка, створіть окремі уроки.");
                return View();
            }

            var lesson = lessonService.Create(model.GroupId, model.SubjectId, model.TeacherId, model.DateTime, model.IsForThematicMarks, model.IsForSemesterMarks, model.IsForYearMarks);
            if (lesson.Data == null)
            {
                foreach (var validationMessage in lesson.ValidationMessages)
                {
                    ModelState.AddModelError(nameof(model.SubjectId), validationMessage);
                }

                return View();
            }

            return RedirectToAction("Fill", new {lessonId = lesson.Data.Id});
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult GroupLessons()
        {
            var currentUser = currentUserProvider.GetCurrentUser<Teacher>(User);
            if (currentUser?.Group == null)
            {
                return View("GroupLessonsEmpty");
            }

            var lessonsForMonth = lessonService.GetLessonsOfGroupBetweenDates(currentUser.Group, DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));

            var converted = lessonsForMonth.ToDictionary(x => x.Key, x => x.Value.Select(value => new LessonListItemModel
            {
                LessonId = value.Id,
                SubjectName = value.Subject.Name,
                IsForThematicMarks = value.IsForThematicMarks,
                IsForSemesterMarks = value.IsForSemesterMarks,
                IsForYearMarks = value.IsForYearMarks
            }));

            return View(converted);
        }

        private bool IsValidType(CreateLessonModel model)
        {
            
            return (model.IsForSemesterMarks ^ model.IsForThematicMarks ^ model.IsForYearMarks) && !(model.IsForSemesterMarks && model.IsForThematicMarks && model.IsForYearMarks)
                || (!model.IsForSemesterMarks && !model.IsForThematicMarks && !model.IsForYearMarks);
        }

        private bool IsTeacherAssociatedForSubject(Teacher teacher, int subjectId)
        {
            return teacher.SubjectRelations != null && teacher.SubjectRelations.Any(x => x.SubjectId == subjectId);
        }

        private bool IsTeacherAssociatedForGroup(Teacher teacher, int groupId)
        {
            return teacher.Group != null && teacher.Group.Id == groupId;
        }
    }

}