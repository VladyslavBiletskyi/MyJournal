using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models.Mark;

namespace MyJournal.WebApi.Controllers
{
    public class MarkController : Controller
    {
        private IMarkService markService;

        public MarkController(IMarkService markService)
        {
            this.markService = markService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Constants.TeacherPolicyName)]
        public IActionResult InsertBatch([FromForm]IEnumerable<LessonMarkModel> model)
        {
            
        }
    }
}