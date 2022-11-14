using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SlackPollingApp.Business.Service;
using SlackPollingApp.Business.Service.ActionHandler.Factory;
using SlackPollingApp.Core.Config;
using SlackPollingApp.Core.Http;
using SlackPollingApp.Hubs;
using SlackPollingApp.Model.Repository;

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
            services.AddSingleton<IPollQueryRepository, PollQueryRepositoryImpl>();
            services.AddSingleton<IPollUpdateRepository, PollUpdateRepositoryImpl>();

            services.AddSingleton<IPollService, PollService>();
            services.AddSingleton<PollService>();
            services.AddSingleton<IActionHandlerFactory, ActionHandlerFactory>();
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
    }
}