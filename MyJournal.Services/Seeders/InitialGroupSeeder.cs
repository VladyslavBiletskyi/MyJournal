using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Seeders;

namespace MyJournal.Services.Seeders
{
    internal class InitialGroupSeeder : IInitialGroupSeeder
    {
        private IGroupRepository groupRepository;

        public InitialGroupSeeder(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public void Seed()
        {
            if (!groupRepository.Instances().Any())
            {
                var group = new Group()
                {
                    Year = 7,
                    Letter = "Б"
                };
                groupRepository.CreateInstance(group);
            }
        }
    }
}
