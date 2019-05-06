using System;

namespace MyJournal.Services.Extensibility.Formatters
{
    public interface IDateTimeFormatter
    {
        string FormatWithoutTime(DateTime dateTime);

        string Format(DateTime dateTime);
    }
}