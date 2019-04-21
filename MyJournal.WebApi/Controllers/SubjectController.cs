using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models.Subject;

namespace MyJournal.WebApi.Controllers
{
    public class SubjectController : Controller
    {
        private ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
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
                ModelState.AddModelError(nameof(model.Name), "Поля заполнены неверно");
                return View();
            }

            if (subjectService.Create(model.Name))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(nameof(model.Name), "Предмет с таким названием уже существует");
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult SelectForAssign()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult Assign(int subjectId)
        {
            var subject = subjectService.Get(subjectId);

            return View();
        }


        [HttpGet]
        public IEnumerable<SubjectModel> Get()
        {
            return subjectService.GetAll().Select(x => new SubjectModel {SubjectId = x.Id, Name = FormatName(x.Name)});
        }

        private string FormatName(string originalName)
        {
            var originalLower = originalName.ToLowerInvariant();
            return originalLower[0].ToString().ToUpperInvariant() + originalLower.Substring(1);
        }
    }
}