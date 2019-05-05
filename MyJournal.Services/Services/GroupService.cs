using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Seeders;
using MyJournal.Services.Extensibility.Services;

namespace MyJournal.Services.Services
{
    internal class GroupService : IGroupService
    {
        private IInitialGroupSeeder initialGroupSeeder;
        private IGroupRepository groupRepository;

        public GroupService(IInitialGroupSeeder initialGroupSeeder, IGroupRepository groupRepository)
        {
            this.initialGroupSeeder = initialGroupSeeder;
            this.groupRepository = groupRepository;
        }

        public IEnumerable<Group> Get()
        {
            initialGroupSeeder.Seed();
            return groupRepository.Instances().ToList();
        }

        public Group Get(int id)
        {
            initialGroupSeeder.Seed();
            return groupRepository.Find(id);
        }

        public bool Create(Group group)
        {
            return groupRepository.CreateInstance(group);
        }
    }
}