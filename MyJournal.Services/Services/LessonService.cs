﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Services.Extensibility.Services;
using MyJournal.Services.Validation;

namespace MyJournal.Services.Services
{
    internal class LessonService : ILessonService
    {
        private ILessonRepository lessonRepository;
        private IGroupRepository groupRepository;
        private ITeacherRepository teacherRepository;
        private ISubjectRepository subjectRepository;

        public LessonService(
            ILessonRepository lessonRepository, IGroupRepository groupRepository, ITeacherRepository teacherRepository, ISubjectRepository subjectRepository)
        {
            this.lessonRepository = lessonRepository;
            this.groupRepository = groupRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
        }

        public ValidationResult<Lesson> Create(int groupId, int subjectId, int teacherId, DateTime dateTime, bool isForThematicMarks, bool isForSemesterMarks, bool isForYearMarks)
        {
            var validationResults = new List<string>();
            var group = groupRepository.Find(groupId);
            if (group == null)
            {
                validationResults.Add("Клас не знайдено");
            }

            var subject = subjectRepository.Find(subjectId);
            if (subject == null)
            {
                validationResults.Add("Предмет не знайдено");
            }

            var teacher = teacherRepository.Find(teacherId);
            if (teacher == null)
            {
                validationResults.Add("Викладач не знайдений");
            }

            Lesson lesson = null;

            if (validationResults.Count == 0)
            {
                lesson = new Lesson
                {
                    DateTime = dateTime,
                    Group = group,
                    Subject = subject,
                    Teacher = teacher,
                    IsForThematicMarks = isForThematicMarks,
                    IsForSemesterMarks = isForSemesterMarks,
                    IsForYearMarks = isForYearMarks
                };
                if (!lessonRepository.CreateInstance(lesson))
                {
                    validationResults.Add("Помилка при створенні уроку");
                    lesson = null;
                }
            }

            return new ValidationResult<Lesson>(lesson, validationResults.ToArray());
        }

        public Lesson Get(int lessonId)
        {
            return lessonRepository.Find(lessonId);
        }

        public IDictionary<DateTime, IEnumerable<Lesson>> GetLessonsOfGroupBetweenDates(Group group, DateTime fromDate, DateTime toDate)
        {
            return lessonRepository.Instances().Where(x => x.Group == group && x.DateTime.Date >= fromDate && x.DateTime <= toDate)
                .GroupBy(x => x.DateTime.Date, x => x)
                .OrderByDescending(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Select(value => value));
        }
    }
}
