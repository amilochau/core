using Microsoft.Extensions.Configuration;
using System;

namespace Milochau.Core.Console
{
    /// <summary>Base startup class for Console applications</summary>
    public abstract class CoreConsoleStartup : CoreStartup
    {
        internal static TStartup Create<TStartup>(IConfiguration configuration)
            where TStartup : CoreConsoleStartup, new()
        {
            var startup = new TStartup();
            startup.SetHostOptions(configuration);
            return startup;
        }

        /// <summary>Configure application</summary>
        public virtual void Configure(IServiceProvider serviceProvider)
        {
        }
    }
}
