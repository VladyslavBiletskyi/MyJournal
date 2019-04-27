using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Models.Subject;

namespace MyJournal.WebApi.Controllers
{
    public class SubjectController : Controller
    {
        private ISubjectService subjectService;
        private IUserService userService;
        private ISubjectNameFormatter subjectNameFormatter;

        public SubjectController(ISubjectService subjectService, IUserService userService, ISubjectNameFormatter subjectNameFormatter)
        {
            this.subjectService = subjectService;
            this.userService = userService;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(model.Name), "Поля заполнені невірно");
                return View();
            }

            if (subjectService.Create(model.Name))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(nameof(model.Name), "Предмет з такою назвою вже існує");
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Assign()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Assign(AssignTeacherModel model)
        {
            var subject = subjectService.Get(model.SubjectId);
            if (subject == null)
            {
                ModelState.AddModelError(nameof(model.SubjectId), "Предмет не знайдено");
                return View();
            }

            var teacher = userService.FindUser(model.TeacherId) as Teacher;
            if (teacher == null)
            {
                ModelState.AddModelError(nameof(model.TeacherId), "Викладача не знайдено");
                return View();
            }

            if (teacher.Subjects.Any(x => x.Id == model.SubjectId))
            {
                ModelState.AddModelError(nameof(model.TeacherId), "Викладач вже зареєстрований на предмет");
                return View();
            }
            teacher.Subjects.Add(subject);
            userService.Update(teacher);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IEnumerable<SubjectModel> Get()
        {
            return subjectService.GetAll().Select(x => new SubjectModel {SubjectId = x.Id, Name = subjectNameFormatter.Format(x)});
        }
    }
}