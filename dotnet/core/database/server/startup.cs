// <copyright file="Startup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Text;
    using Database.Domain;
    using Services;
    using Security;
    using JSNLog;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using ObjectFactory = Database.ObjectFactory;
    using Database.Adapters;
    using Database.Configuration;
    using Database.Configuration.Derivations.Default;
    using Database.Meta;
    using User = Database.Domain.User;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);

            var workspaceConfig = new WorkspaceConfig(new Dictionary<HostString, string>
            {
                {new HostString("localhost", 5000), "Default"}
            });

            // Allors
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton(workspaceConfig);
            // Allors Scoped
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();

            services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder => builder
                        .WithOrigins("http://localhost", "http://localhost:4000", "http://localhost:4200", "http://localhost:9876")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            services.AddDefaultIdentity<IdentityUser>()
                .AddAllorsStores();

            services.AddAuthentication(option => option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Configuration.GetSection("JwtToken:Key").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    });

            services.AddResponseCaching();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            // Allors
            var metaPopulation = new MetaBuilder().Build();
            var objectFactory = new ObjectFactory(metaPopulation, typeof(User));
            var databaseScope = new DefaultDatabaseServices(httpContextAccessor);
            var databaseBuilder = new DatabaseBuilder(databaseScope, this.Configuration, objectFactory);
            app.ApplicationServices.GetRequiredService<IDatabaseService>().Database = databaseBuilder.Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();

            var jsnlogConfiguration = new JsnlogConfiguration
            {
                corsAllowedOriginsRegex = ".*",
                serverSideMessageFormat = env.IsDevelopment() ?
                                            "%requestId | %url | %message" :
                                            "%requestId | %url | %userHostAddress | %userAgent | %message",
            };

            app.UseJSNLog(new LoggingAdapter(loggerFactory), jsnlogConfiguration);

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureExceptionHandler(env, loggerFactory);

            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "allors/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
