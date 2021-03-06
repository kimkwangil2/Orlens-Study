﻿
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PS.Applications.Common.Interfaces;
using PS.Infrasture.Identity;
using PS.Infrasture.Persistence;
using PS.Infrasture.Persistence2;
using PS.Infrasture.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PS.Infrasture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDbContext<MechantDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection2")));


            services.AddScoped<IMechantDbContext>(provider => provider.GetService<MechantDbContext>());


            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            if (environment.IsEnvironment("development"))
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

                services.AddTransient<IDateTime, DateTimeService>();
                services.AddTransient<IIdentityService, IdentityService>();
                //services.AddIdentityServer()
                //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                //    {
                //        options.Clients.Add(new Client
                //        {
                //            ClientId = "Productsystem.IntegrationTests",
                //            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                //            ClientSecrets = { new Secret("secret".Sha256()) },
                //            AllowedScopes = { "CleanArchitecture.WebUIAPI", "openid", "profile" }
                //        });
                //    }).AddTestUsers(new List<TestUser>
                //    {
                //        new TestUser
                //        {
                //            SubjectId = "f26da293-02fb-4c90-be75-e4aa51e0bb17",
                //            Username = "jason@clean-architecture",
                //            Password = "CleanArchitecture!",
                //            Claims = new List<Claim>
                //            {
                //                new Claim(JwtClaimTypes.Email, "jason@clean-architecture")
                //            }
                //        }
                //    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

                services.AddTransient<IDateTime, DateTimeService>();
                services.AddTransient<IIdentityService, IdentityService>();
                
            }

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            return services;
        }
    }
}
