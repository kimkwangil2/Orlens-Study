using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using Orleans.Storage;
using PS.Applications;
using PS.Applications.Common.Interfaces;
using PS.Infrasture;
using PS.Silo.HostServer.Services;
using Microsoft.Extensions.Logging;
using PS.Common.OrlensHelper;
using PS.Infrasture.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using PS.Infrasture.Identity;
using PS.Infrasture.Services;
using OCatle.inf.V1.Streaming;

namespace PS.Silo.HostServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public IGrainStorage Storage { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddApplication();
            //services.AddInfrastructure(Configuration, Environment);
            //services.AddScoped<ICurrentUserService, CurrentUserService>();

            //�������� 
            services.AddAuthorization(); 
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
                                               //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>())
                                               //.AddNewtonsoftJson();
            services.AddRazorPages();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //services.AddSingleton<IOrlensConfigurator, OrlensConfigurator>();
            //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(GetLogger(), true));

        }
         

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�̵����
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            //ũ�ν� ����¡ ��å
            app.UseCors();
            //����� ���
            app.UseRouting();
            //�̵��� ������ ���� �ο�
            app.UseAuthorization();
            //����� ��Ģ
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            }); 

            await StartSilo();
        }

        private async Task<ISiloHost> StartSilo()
        {  
            OrlensConfigurator ofi = new OrlensConfigurator(Configuration);

            var dash = ofi.GetDashBoard();
            //var lstdb = ofi.RegDatabaseContext();
            var builder = new SiloHostBuilder()
              .ConfigureClustering(
                  ofi.GetOrlConfig(),
                  ofi.GetEnvironmentString()
              )
              .Configure<ClusterOptions>(options =>
              {
                  options.ClusterId = "Siloserver";
                  options.ServiceId = "OcatleService";
              })
                .UseLocalhostClustering()
                .AddMemoryGrainStorage(StreamNames.PubSubStorageName)
                //.AddMemoryGrainStorage(Constants.OrleansMemoryProvider)
                .UseInMemoryReminderService()                
                .ConfigureServices(services =>
                {
                    services.AddApplication();
                    services.AddInfrastructure(Configuration, Environment);
                    services.AddScoped<ICurrentUserService, CurrentUserService>();
                    //services..AddFluentVa1lidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IApplicationDbContext>());


                })
                //.ConfigureApplicationParts(parts =>
                //{
                //    parts.AddApplicationPart(typeof(IGrainInterfaceMarker).Assembly).WithReferences();
                //})                 
                //.ConfigureServices(DependencyInjectionHelper.IocContainerRegistration)                 
                .UsePerfCounterEnvironmentStatistics()
                //.UseDashboard
                //(options =>
                //{
                //    options.Username = dash.UserName;
                //    options.Password = dash.Password;
                //    options.Host = dash.Host;
                //    options.Port = dash.Port;
                //    options.HostSelf = true;
                //    options.CounterUpdateIntervalMs = dash.CounterUpdateIntervalMs;
                //})
                .ConfigureLogging(logging => logging.AddConsole().AddSerilog())
                .AddSimpleMessageStreamProvider(StreamNames.PubSubProviderName)
                ;

            var host = builder.Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }

            await host.StartAsync();
            return host;
        }
    }
}