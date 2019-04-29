﻿using System;
using System.Collections.Generic;
using MyJournal.Domain.Entities;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Extensibility.Services
{
    public interface IMarkService
    {
        ValidationResult InsertBatch(IEnumerable<Mark> marks);

        IDictionary<DateTime, IEnumerable<Mark>> GetMarks(Student student, DateTime fromDay, DateTime toDay);
    }
}
