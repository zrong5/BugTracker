using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Service;

namespace BugTracker
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
            services.AddSingleton(Configuration);
            services.AddScoped<IBug, BugService>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IChart, ChartService>();
            services.AddScoped<IUserBug, UserBugService>();
            // services.AddDbContext<TrackerContext>(options
            // => options.UseSqlServer(Configuration.GetConnectionString("TrackerConnection")));
            services.AddDbContext<TrackerContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<TrackerContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("View Dashboard", policy => policy.RequireRole("Developer", "Admin", "Manager"));
                options.AddPolicy("Manage Roles", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Manage Users", policy => policy.RequireRole("Manager", "Admin"));
                options.AddPolicy("Manage Projects", policy => policy.RequireRole("Manager", "Admin"));
                options.AddPolicy("View Bugs", policy => policy.RequireRole("Developer", "Manager", "Admin"));
                options.AddPolicy("Submit Bugs", policy => policy.RequireRole("Developer", "Admin", "Manager", "Submitter"));
                options.AddPolicy("Submitter Permission", policy => policy.RequireRole("Developer", "Admin", "Manager", "Submitter"));
                options.AddPolicy("Developer Permission", policy => policy.RequireRole("Developer", "Admin", "Manager"));
                options.AddPolicy("Manager Permission", policy => policy.RequireRole("Admin", "Manager"));
                options.AddPolicy("Admin Permission", policy => policy.RequireRole("Admin"));
                options.AddPolicy("View Profile", policy => policy.RequireRole("Developer", "Admin", "Manager", "Submitter"));
                // options.AddPolicy("Add Role", policy => policy.RequireClaim("Can add roles", "add.role"));
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Identity.Cookie";
                options.LoginPath = "/Home/Login";
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
