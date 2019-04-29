using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Data;
using MyJournal.Domain.Extensibility;
using MyJournal.Domain.Extensibility.Repositories;
using MyJournal.Domain.Repositories;

namespace MyJournal.Domain
{
    public class DomainBindingModule: IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<ILessonRepository, LessonRepository>();
            services.AddTransient<ILessonSkipRepository, LessonSkipRepository>();
            services.AddTransient<IMarkRepository, MarkRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<ITeacherRepository, TeacherRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ITeacherSubjectRelationshipRepository, TeacherSubjectRelationshipRepository>();
        }
    }
}