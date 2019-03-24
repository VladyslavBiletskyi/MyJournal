using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
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
            services.AddTransient<IAttendRepository, AttendRepository>();
            services.AddTransient<IMarkRepository, MarkRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
        }
    }
}