using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Console
{
    /// <summary>Base entry point for Console application</summary>
    public abstract class CoreConsoleEntryPoint
    {
        /// <summary>Main console application entry point</summary>
        /// <returns>Exit code</returns>
        public abstract Task<int> RunAsync(CancellationToken cancellationToken);
    }
}
