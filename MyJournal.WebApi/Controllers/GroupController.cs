using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyJournal.Domain.Entities;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Models;

namespace MyJournal.WebApi.Controllers
{
    public class GroupController : Controller
    {
        private IGroupService groupService;
        private ILogger logger;

        public GroupController(IGroupService groupService, ILogger<GroupController> logger)
        {
            this.groupService = groupService;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<GroupModel> GetAll()
        {
            try
            {
                return groupService.Get().Select(x => new GroupModel {GroupId = x.Id, Name = FormatGroupName(x)});
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return null;
            }
        }

        private string FormatGroupName(Group group)
        {
            return group.Letter != null ? $"{group.Year}-{group.Letter}" : group.Year.ToString();
        }
    }
}