﻿// <copyright file="HealthTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Health.Benchmarks.Fixtures
{
    public class HealthTestFixture : IDisposable
    {
        public HealthTestFixture()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHealthChecks().AddChecks(
                registry => { registry.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())); });

            var provider = services.BuildServiceProvider();

            Health = provider.GetRequiredService<IProvideHealth>();
        }

        public IProvideHealth Health { get; }

        public void Dispose() { }
    }
}