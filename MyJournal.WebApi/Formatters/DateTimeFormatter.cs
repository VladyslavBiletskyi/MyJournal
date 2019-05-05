using System;
using MyJournal.WebApi.Extensibility.Formatters;

namespace MyJournal.WebApi.Formatters
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
