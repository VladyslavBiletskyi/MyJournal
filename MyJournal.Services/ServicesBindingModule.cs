using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
using MyJournal.Services.Extensibility;
using MyJournal.Services.Extensibility.Seeders;
using MyJournal.Services.Extensibility.Services;
using MyJournal.Services.Seeders;
using MyJournal.Services.Services;

namespace MyJournal.Services
{
    public class ServicesBindingModule: IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IInitialUserSeeder, InitialUserSeeder>();
            services.AddTransient<IInitialGroupSeeder, InitialGroupSeeder>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<ISubjectService, SubjectService>();
        }
    }
}