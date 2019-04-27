using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models.User;

namespace MyJournal.WebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        
        public IEnumerable<TeacherModel> GetTeachers()
        {
            return userService.GetTeachers().Select(x => new TeacherModel { TeacherId = x.Id, Name = $"{ x.Surname } { x.FirstName } { x.LastName }" });
        }
    }
}