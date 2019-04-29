using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    public class MarkController : Controller
    {
        private readonly IMarkService markService;
        private readonly IUserService userService;
        private readonly ILessonService lessonService;
        private readonly ICurrentUserProvider currentUserProvider;

        public MarkController(IMarkService markService, IUserService userService, ILessonService lessonService, ICurrentUserProvider currentUserProvider)
        {
            this.markService = markService;
            this.userService = userService;
            this.lessonService = lessonService;
            this.currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult MarksByWeek()
        {
            var user = currentUserProvider.GetCurrentUser<Student>(User);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult InsertBatch([FromForm]IEnumerable<LessonMarkModel> model)
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
                    Grade = x.Mark ?? -1,
                    Lesson = lesson,
                    Student = student,
                    UpdateTime = DateTime.Now
                };
            }).ToList();

            markService.InsertBatch(marksForSaving);
            return RedirectToAction("Index", "Lesson");
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