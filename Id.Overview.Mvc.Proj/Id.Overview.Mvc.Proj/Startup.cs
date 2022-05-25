using Id.Overview.Mvc.Proj.Data;
using Id.Overview.Mvc.Proj.Models;
using Id.Overview.Mvc.Proj.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id.Overview.Mvc.Proj
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                //lockout
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                //password
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                //Sign In
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                //Tokens

                // options.Tokens.AuthenticatorTokenProvider
                // options.Tokens.ChangeEmailTokenProvider
                // options.Tokens.ChangePhoneNumberTokenProvider
                // options.Tokens.EmailConfirmationTokenProvider
                // options.Tokens.PasswordResetTokenProvider

                //User

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-.@+";
                //Valor Default: False
                options.User.RequireUniqueEmail = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                // options.ClaimsIssuer = "";
                // Local - options.Cookie.Domain = "";
                //options.Cookie.Expiration = "";
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".AspNetCore.Cookies";
                //options.Cookie.Path = "";
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                // options.CookieManager = 
                //options.DataProtectionProvider
                //options.Events
                // options.EventsType = 
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ReturnUrlParameter = "ReturnUrl";
                // options.SessionStore = "";
                options.SlidingExpiration = true;
                // options.TicketDataFormat = 
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
