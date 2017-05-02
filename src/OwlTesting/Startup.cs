using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DAL;
using DAL.Interface;
using BAL.Interface;
using Microsoft.EntityFrameworkCore;
using BAL.Manager;
using Model.DB;
using BAL;
using DAL.Repositories;

namespace OwlTesting
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            AutoMapperConfig.Configure();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<MainContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MainContext>()
                .AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;

				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;

				// Cookie settings
				options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
				options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
				options.Cookies.ApplicationCookie.LogoutPath = "/Account/SignOff";

				// User settings
				options.User.RequireUniqueEmail = true;
			});

			services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISubjectManager, SubjectManager>();
            services.AddTransient<ITestManager, TestManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IQuestionManager, QuestionManager>();
            services.AddTransient<ITestResultRepository, TestResultRepository>();
            services.AddTransient<ITestResultManager, TestResultManager>();
			services.AddTransient<ISubjectRepository, SubjectRepository>();
			services.AddTransient<IAnswerRepository, AnswerRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
