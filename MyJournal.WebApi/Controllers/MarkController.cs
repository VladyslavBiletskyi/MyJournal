using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    public class MarkController : Controller
    {
        private IMarkService markService;
        private IUserService userService;
        private ILessonService lessonService;

        public MarkController(IMarkService markService, IUserService userService, ILessonService lessonService)
        {
            this.markService = markService;
            this.userService = userService;
            this.lessonService = lessonService;
        }

        public IActionResult Index()
        {
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

            var marksForSaving = model.Select(x => new Mark
            {
                Attend = x.NotPresent ? null : new Attend()
                {
                    Student = userService.FindStudent(x.StudentId),
                    Lesson = lesson
                },
                Grade = x.Mark ?? -1,
                UpdateTime = DateTime.Now
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