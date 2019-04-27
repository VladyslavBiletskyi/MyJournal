using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Services
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetTeachers();

        IEnumerable<ApplicationUser> GetStudents();

        IEnumerable<ApplicationUser> GetUsers();

        ApplicationUser FindUser(int id);

        ApplicationUser FindUser(string login);

        bool Update(ApplicationUser user);
    }
}