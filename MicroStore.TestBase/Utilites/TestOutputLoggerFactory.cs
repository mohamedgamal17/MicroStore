﻿using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Catalog.Domain.Tests.Utilites
{
    [ExposeServices(typeof(ILoggerFactory), IncludeSelf = true)]
    public class TestOutputLoggerFactory :
            ILoggerFactory , ITransientDependency
    {
        readonly bool _enabled;

        public TestOutputLoggerFactory(bool enabled)
        {
            _enabled = enabled;
        }

        public TestExecutionContext Current { get; set; }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string name)
        {
            return new TestOutputLogger(this, _enabled);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }
    }
}