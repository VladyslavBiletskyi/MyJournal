using System;
using MyJournal.Domain.Entities;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Extensibility.Services
{
    public interface ILessonService
    {
        ValidationResult<Lesson> Create(int groupId, int subjectId, int teacherId, DateTime dateTime);

        Lesson Get(int lessonId);
    }
}