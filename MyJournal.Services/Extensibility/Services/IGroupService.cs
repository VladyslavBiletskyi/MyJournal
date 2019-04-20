using System.Collections.Generic;
using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Services
{
    public interface IGroupService
    {
        IEnumerable<Group> Get();

        Group Get(int id);
    }
}
