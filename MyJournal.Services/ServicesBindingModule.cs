using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Extensibility;
using MyJournal.Services.Extensibility;
using MyJournal.Services.Extensibility.Seeders;
using MyJournal.Services.Seeders;

namespace MyJournal.Services
{
    public class ServicesBindingModule: IBindingModule
    {
        public void ApplyBindings(IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IInitialUserSeeder, InitialUserSeeder>();
            services.AddTransient<IUserManager, UserManager>();
        }
    }
}