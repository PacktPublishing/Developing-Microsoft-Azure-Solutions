using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public interface IDependencies<T> where T: class
    {
        ILogger Logger { get; }
        AppSettings Settings { get; }
    }

    public abstract class Dependencies<T> : IDependencies<T> where T : class
    {
        public Dependencies(ILogger<T> logger, AppSettings settings)
        {
            CheckIsNotNull(nameof(logger), logger);
            CheckIsNotNull(nameof(settings), settings);

            Logger = logger;
            Settings = settings;
        }

        public AppSettings Settings { get; }

        public ILogger Logger { get; }
    }
}
