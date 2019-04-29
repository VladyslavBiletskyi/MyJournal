using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
using MyJournal.WebApi.Controllers;
using MyJournal.WebApi.Extensibility.Formatters;
using MyJournal.WebApi.Extensibility.Providers;
using MyJournal.WebApi.Formatters;
using MyJournal.WebApi.Providers;

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
            services.AddTransient<IUserNameFormatter, UserNameFormatter>();

            services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();
        }
    }
}
