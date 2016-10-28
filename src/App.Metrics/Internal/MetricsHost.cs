// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    internal class MetricsHost : IMetricsHost
    {
        internal MetricsHost(IServiceCollection services, IMetricsEnvironment environment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (environment == null) throw new ArgumentNullException(nameof(environment));

            Services = services;
            Environment = environment;
        }

        public IMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }
    }
}