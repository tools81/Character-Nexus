using Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CharacterNexus
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
            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSWA", policy =>
                {
                    policy
                        .WithOrigins("https://agreeable-cliff-002808f10.4.azurestaticapps.net") // SWA domain
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                options.AddPolicy("DevCors", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSingleton(configuration);

            services.AddSingleton<IRuleset, AmazingTales.Ruleset>();
            // services.AddSingleton<IRuleset, BladeRunner.Ruleset>();
            services.AddSingleton<IRuleset, DarkCrystal.Ruleset>();
            services.AddSingleton<IRuleset, EverydayHeroes.Ruleset>();
            // services.AddSingleton<IRuleset, Ghostbusters.Ruleset>();
            //services.AddSingleton<IRuleset, GiJoe.Ruleset>();
            services.AddSingleton<IRuleset, Marvel.Ruleset>();
            //services.AddSingleton<IRuleset, Starfinder.Ruleset>();
            //services.AddSingleton<IRuleset, Transformers.Ruleset>();
            //services.AddSingleton<IRuleset, VampireTheMasquerade.Ruleset>();
            //services.AddSingleton<IRuleset, WerewolfTheApocalypse.Ruleset>();

            services.AddSingleton<IStorage, AzureBlobStorage.Storage>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowSWA");
            app.UseCors("DevCors");

            app.UseRulesetMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
