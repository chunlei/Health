﻿// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Sandbox
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app.UseHealthChecks();

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.
                AddHealthChecks().
                AddHealthCheckMiddleware(optionsBuilder => optionsBuilder.AddAsciiFormatters()).
                AddChecks(
                    registry =>
                    {
                        registry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                        registry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                    });
        }
    }
}