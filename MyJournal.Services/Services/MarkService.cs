﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Services
{
    internal class MarkService : IMarkService
    {
        private IMarkRepository markRepository;
        private ILessonSkipRepository lessonSkipRepository;

        public MarkService(IMarkRepository markRepository, ILessonSkipRepository lessonSkipRepository)
        {
            this.markRepository = markRepository;
            this.lessonSkipRepository = lessonSkipRepository;
        }

        public IDictionary<DateTime, IEnumerable<Mark>> GetMarks(Student student, DateTime fromDay, DateTime toDay)
        {
            var userMarks = markRepository.Instances().Where(x => x.Student == student && x.Lesson.DateTime >= fromDay && x.Lesson.DateTime < toDay).ToList();
            return userMarks.GroupBy(x => x.Lesson.DateTime.Date, x => x).ToDictionary(x => x.Key, x => x.Select(value => value));
        }

        public ValidationResult InsertBatch(IEnumerable<Mark> marks)
        {
            var validationResults = new List<string>();
            marks = marks.ToList();
            if (!lessonSkipRepository.BatchInsert(marks.Select(x => x.LessonSkip).Where(x => x != null)))
            {
                validationResults.Add("Помилка при реєстрації пропусків");
            }
            if (!markRepository.BatchInsert(marks.Where(mark => mark.Grade >=0)))
            {
                validationResults.Add("Помилка при виставленні оцінок");
            }

            return new ValidationResult(validationResults.ToArray());
        }
    }
}
