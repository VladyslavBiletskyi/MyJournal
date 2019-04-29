using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Models.Lesson;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    public class LessonController : Controller
    {
        private readonly IUserService userService;
        private readonly ILessonService lessonService;
        private readonly IGroupNameFormatter groupNameFormatter;
        private readonly ISubjectNameFormatter subjectNameFormatter;
        private readonly ICurrentUserProvider currentUserProvider;

        public LessonController(
            IUserService userService, ILessonService lessonService, IGroupNameFormatter groupNameFormatter, ISubjectNameFormatter subjectNameFormatter, ICurrentUserProvider currentUserProvider)
        {
            this.userService = userService;
            this.lessonService = lessonService;
            this.groupNameFormatter = groupNameFormatter;
            this.subjectNameFormatter = subjectNameFormatter;
            this.currentUserProvider = currentUserProvider;
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
            return DisplayInternal(lessonId, "Display");
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Fill(int lessonId)
        {
            return DisplayInternal(lessonId, "Fill");
        }

        private IActionResult DisplayInternal(int lessonId, string viewName)
        {
            var lesson = lessonService.Get(lessonId);
            if (lesson == null || lesson.Group == null)
            {
                return RedirectToAction("Index");
            }

            var markModels = lesson.Group.Students?.Select(x => new LessonMarkModel
            {
                LessonId = lessonId,
                Mark = null,
                NotPresent = false,
                StudentId = x.Id,
                StudentName = $"{x.Surname} {x.FirstName} {x.LastName}"
            }) ?? new List<LessonMarkModel>();

            return View(viewName, new LessonModel
            {
                LessonId = lessonId,
                MarksData = markModels,
                GroupName = groupNameFormatter.Format(lesson.Group),
                SubjectName = subjectNameFormatter.Format(lesson.Subject)
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

            if (!(IsTeacherAssociatedForGroup(teacher, model.GroupId) || IsTeacherAssociatedForSubject(teacher, model.SubjectId)))
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

            var lesson = lessonService.Create(model.GroupId, model.SubjectId, model.TeacherId, model.DateTime);
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

        private bool IsTeacherAssociatedForSubject(Teacher teacher, int subjectId)
        {
            return teacher.Subjects != null && teacher.Subjects.Any(x => x.Id == subjectId);
        }

        private bool IsTeacherAssociatedForGroup(Teacher teacher, int groupId)
        {
            return teacher.Group != null && teacher.Group.Id == groupId;
        }
    }

}