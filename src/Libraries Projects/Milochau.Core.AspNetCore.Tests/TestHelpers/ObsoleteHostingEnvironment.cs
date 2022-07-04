using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
#pragma warning disable CS0618 // Le type ou le membre est obsolète
    internal class ObsoleteHostingEnvironment : IHostingEnvironment
#pragma warning restore CS0618 // Le type ou le membre est obsolète
    {
        public string EnvironmentName { get; set; } = null!;
        public string ApplicationName { get; set; } = null!;
        public string WebRootPath { get; set; } = null!;
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public string ContentRootPath { get; set; } = null!;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
