using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Models;
using MyJournal.WebApi.Models.Group;

namespace MyJournal.WebApi.Controllers
{
    public class GroupController : Controller
    {
        private IGroupService groupService;
        private ILogger logger;
        private IGroupNameFormatter groupNameFormatter;
        private IUserService userService;

        public GroupController(IGroupService groupService, ILogger<GroupController> logger, IGroupNameFormatter groupNameFormatter, IUserService userService)
        {
            this.groupService = groupService;
            this.logger = logger;
            this.groupNameFormatter = groupNameFormatter;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<GroupModel> GetAll()
        {
            try
            {
                return groupService.Get().Select(x => new GroupModel {GroupId = x.Id, Name = groupNameFormatter.Format(x)});
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Create(CreateGroupModel model)
        {
            if (model.Year < 1 || model.Year > 11)
            {
                ModelState.AddModelError(nameof(model.Year), "Вказано невірний рік навчання");
                return View();
            }
            var groups = groupService.Get().Where(g => g.Year == model.Year);
            if (groups.Any(x => x.Letter.ToUpperInvariant() == model.Letter.ToUpperInvariant()))
            {
                ModelState.AddModelError(nameof(model.Letter), "Клас з такою буквою вже існує");
                return View();
            }
            var group = new Group {Letter = model.Letter.ToUpperInvariant(), Year = model.Year};
            if (!groupService.Create(group))
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult AssignTeacher()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult AssignTeacher(AssignTeacherModel model)
        {
            if (model.GroupId < 0)
            {
                ModelState.AddModelError(nameof(model.GroupId),"Клас не вибрано");
                return View();
            }

            var group = groupService.Get(model.GroupId);
            var teacher = userService.FindTeacher(model.TeacherId);
            if (teacher == null)
            {
                ModelState.AddModelError(nameof(model.GroupId), "Викладача не знайдено");
                return View();
            }
            if (teacher.Group != null)
            {
                ModelState.AddModelError(nameof(model.GroupId), "Викладач вже має клас");
                return View();
            }

            teacher.Group = group;
            if (!userService.Update(teacher))
            {
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}