using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Models.User;

namespace MyJournal.WebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserNameFormatter userNameFormatter;

        public UserController(IUserService userService, IUserNameFormatter userNameFormatter)
        {
            this.userService = userService;
            this.userNameFormatter = userNameFormatter;
        }

        [HttpGet]
        public IEnumerable<TeacherModel> GetTeachers()
        {
            return userService.GetTeachers().Select(x => new TeacherModel { TeacherId = x.Id, Name = userNameFormatter.FormatFull(x) });
        }

        [HttpGet]
        public IEnumerable<UserModel> GetAllUsers()
        {
            return userService.GetUsers().OrderBy(x => x.Surname).Select(x => new UserModel {Name = userNameFormatter.FormatFull(x), Id = x.Id});
        }
    }
}