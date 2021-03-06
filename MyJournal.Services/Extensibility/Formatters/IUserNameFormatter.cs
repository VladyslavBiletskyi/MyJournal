﻿using MyJournal.Domain.Entities;

namespace MyJournal.Services.Extensibility.Formatters
{
    public interface IUserNameFormatter
    {
        string FormatFull(ApplicationUser user);

        string FormatWithoutSurname(ApplicationUser user);

        string FormatShort(ApplicationUser user);
    }
}
