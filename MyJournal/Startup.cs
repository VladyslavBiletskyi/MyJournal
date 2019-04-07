using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyJournal.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyJournal.Domain.Entities;
using MyJournal.Domain.Extensibility;

namespace MyJournal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MyJournalDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<MyJournalDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ApplyBindings(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            AddClaims(userManager, roleManager);
        }

        private void ApplyBindings(IServiceCollection services)
        {
            services.AddSingleton<IDatabaseContext, MyJournalDbContext>();

            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblies = new List<Assembly> { entryAssembly };

            foreach (var assembly in entryAssembly.GetReferencedAssemblies())
            {
                assemblies.Add(Assembly.Load(assembly));
            }

            foreach (var assembly in assemblies.Where(x => x.FullName.StartsWith(entryAssembly.GetName().Name)))
            {
                foreach (TypeInfo ti in assembly.DefinedTypes)
                {
                    if (ti.ImplementedInterfaces.Contains(typeof(IBindingModule)))
                    {
                        (assembly.CreateInstance(ti.FullName) as IBindingModule)?.ApplyBindings(services);
                    }
                }
            }
        }

        private void AddClaims(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Teacher").Result)
            {
                var result = roleManager.CreateAsync(new IdentityRole("Teacher")).Result;
            }
        }
    }
}
