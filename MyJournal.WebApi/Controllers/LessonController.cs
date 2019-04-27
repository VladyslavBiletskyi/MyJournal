﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Models.Lesson;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    public class LessonController : Controller
    {
        private IUserService userService;
        private ILessonService lessonService;
        private IGroupNameFormatter groupNameFormatter;
        private ISubjectNameFormatter subjectNameFormatter;

        public LessonController(IUserService userService, ILessonService lessonService, IGroupNameFormatter groupNameFormatter, ISubjectNameFormatter subjectNameFormatter)
        {
            this.userService = userService;
            this.lessonService = lessonService;
            this.groupNameFormatter = groupNameFormatter;
            this.subjectNameFormatter = subjectNameFormatter;
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
            var login = User.Claims.FirstOrDefault(x => x.Type == Constants.UserLoginClaimName)?.Value;
            var teacher = userService.FindUser(login) as Teacher;
            if (teacher == null || !(IsTeacherAssociatedForGroup(teacher, model.GroupId) || IsTeacherAssociatedForSubject(teacher, model.SubjectId)))
            {
                return RedirectToAction("Index");
            }

            var lessonTeacher = userService.FindUser(model.TeacherId) as Teacher;
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