﻿using System;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal.Domain.Repositories
{
    public class MarkRepository : RepositoryBase<Mark>, IMarkRepository
    {
        public MarkRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override bool TryUpdateInstance(Mark instance)
        {
            var original = Find(instance.Id);
            if (original == null)
            {
                return false;
            }

            original.Attend = instance.Attend;
            original.Grade = instance.Grade;
            original.UpdateTime = DateTime.Now;
            DatabaseContext.SaveChanges();
            return true;
        }
    }
}
