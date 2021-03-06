﻿using System.Collections.Generic;
using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Services
{
    public interface ISubjectService
    {
        IEnumerable<Subject> GetAll();

        IEnumerable<Subject> GetSubjectsOfGroup(Group group);

        Subject Get(int id);

        bool Create(string name);
    }
}