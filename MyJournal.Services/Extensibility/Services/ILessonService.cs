using System;
using System.Collections.Generic;
using MyJournal.Domain.Entities;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Extensibility.Services
{
    public interface ILessonService
    {
        ValidationResult<Lesson> Create(int groupId, int subjectId, int teacherId, DateTime dateTime, bool isForThematicMarks);

        IDictionary<DateTime, IEnumerable<Lesson>> GetLessonsOfGroupBetweenDates(Group group, DateTime fromDate, DateTime toDate);

        Lesson Get(int lessonId);
    }
}