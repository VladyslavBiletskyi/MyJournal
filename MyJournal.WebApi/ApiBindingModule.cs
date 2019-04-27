using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
using MyJournal.WebApi.Controllers;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Formatters;

namespace MyJournal.WebApi
{
    public class ApiBindingModule : IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            services.AddTransient<GroupController>();
            services.AddTransient<SubjectController>();
            services.AddTransient<UserController>();

            services.AddTransient<IGroupNameFormatter, GroupNameFormatter>();
            services.AddTransient<ISubjectNameFormatter, SubjectNameFormatter>();
        }
    }
}
