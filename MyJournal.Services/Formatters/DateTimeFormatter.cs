using System;
using MyJournal.Services.Extensibility.Formatters;

namespace MyJournal.Services.Formatters
{
    public class DateTimeFormatter : IDateTimeFormatter
    {
        public string FormatWithoutTime(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM");
        }

        public string Format(DateTime dateTime)
        {
            return $"{FormatWithoutTime(dateTime)} {dateTime:HH:mm}";
        }
    }
}
