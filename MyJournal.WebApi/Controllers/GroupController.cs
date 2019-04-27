using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyJournal.Services.Extensibility.Services;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Models;

namespace MyJournal.WebApi.Controllers
{
    public class GroupController : Controller
    {
        private IGroupService groupService;
        private ILogger logger;
        private IGroupNameFormatter groupNameFormatter;

        public GroupController(IGroupService groupService, ILogger<GroupController> logger, IGroupNameFormatter groupNameFormatter)
        {
            this.groupService = groupService;
            this.logger = logger;
            this.groupNameFormatter = groupNameFormatter;
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
    }
}