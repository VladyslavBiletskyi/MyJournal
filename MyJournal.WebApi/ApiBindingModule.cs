using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
using MyJournal.WebApi.Controllers;

namespace MyJournal.WebApi
{
    public class ApiBindingModule : IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            services.AddTransient<GroupController>();
            services.AddTransient<SubjectController>();
            services.AddTransient<UserController>();
        }
    }
}
