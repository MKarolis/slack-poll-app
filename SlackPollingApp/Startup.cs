using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SlackPollingApp.Business.Service;
using SlackPollingApp.Core.Config;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Model.Repository;
using Microsoft.AspNetCore.ResponseCompression;
using SlackPollingApp.Hubs;

namespace SlackPollingApp
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.Configure<SlackConfig>(Configuration.GetSection("SlackConfig"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("MongoConnection"));
            
            services.AddHttpClient();
            services.AddSingleton<IHttpRequestSender, HttpRequestSender>();
            services.AddSingleton<HttpRequestSender>();
            
            services.AddSingleton<MongoContext>();
            services.AddSingleton<PollRepository>();

            services.AddSingleton<IPollService, PollService>();
            services.AddSingleton<PollService>();
            services.AddSingleton<ActionService>();
            services.AddSingleton<SlashCommandService>();
            services.AddSingleton<AuthService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<NotificationService>();
            
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            
            services.AddProtectedBrowserStorage();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "SlackPollingApp", Version = "v1"});
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SlackPollingApp v1"));
                app.UseHsts();
            }
            
            app.UseRouting();
            
            app.UseStaticFiles();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHub<PollHub>(PollHub.HubUrl);
                endpoints.MapRazorPages();
            });
        }

        private void ConfigureOpenId(IServiceCollection services)
        {
            // services.AddAuthentication(options =>
            //     {
            //         options.DefaultScheme = "Cookies";
            //         options.DefaultChallengeScheme = "oidc";
            //     })
            //     .AddCookie("Cookies")
            //     .AddOpenIdConnect("oidc", options =>
            //     {
            //         options.Authority = "https://demo.identityserver.io/";
            //         options.ClientId = "interactive.confidential.short"; // 75 seconds
            //         options.ClientSecret = "secret";
            //         options.ResponseType = "code";
            //         options.SaveTokens = true;
            //         options.GetClaimsFromUserInfoEndpoint = true;
            //
            //         options.Events = new OpenIdConnectEvents
            //         {
            //             OnAccessDenied = context =>
            //             {
            //                 context.HandleResponse();
            //                 context.Response.Redirect("/");
            //                 return Task.CompletedTask;
            //             }
            //         };
            //     });
            
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie().AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://slack.com/openid/connect/authorize/";
                options.ResponseType = "code";
                options.SaveTokens = true;
                // options.GetClaimsFromUserInfoEndpoint = true;
                // options.UseTokenLifetime = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.TokenValidationParameters = new TokenValidationParameters{ NameClaimType = "name" };
            
                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
            });
            // services.AddMvcCore(options =>
            // {
            //     var policy = new AuthorizationPolicyBuilder()
            //         .RequireAuthenticatedUser()
            //         .Build();
            //     options.Filters.Add(new AuthorizeFilter(policy));
            // });
            
        }
    }
}