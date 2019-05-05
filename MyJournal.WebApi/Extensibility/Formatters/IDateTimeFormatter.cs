using System;

namespace MyJournal.WebApi.Extensibility.Formatters
{
    public interface IDateTimeFormatter
    {
        string FormatWithoutTime(DateTime dateTime);

        string Format(DateTime dateTime);
    }
}