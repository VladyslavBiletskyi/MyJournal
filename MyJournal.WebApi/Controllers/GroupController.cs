using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models;

namespace MyJournal.WebApi.Controllers
{
    public class GroupController : Controller
    {
        private IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public IEnumerable<GroupModel> GetAll()
        {
            return groupService.Get().Select(x => new GroupModel{GroupId = x.Id, Name = FormatGroupName(x)});
        }

        private string FormatGroupName(Group group)
        {
            return group.Letter != null ? $"{group.Year}-{group.Letter}" : group.Year.ToString();
        }
    }
}