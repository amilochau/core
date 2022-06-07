using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
#pragma warning disable CS0618 // Le type ou le membre est obsolète
    internal class ObsoleteHostingEnvironment : IHostingEnvironment
#pragma warning restore CS0618 // Le type ou le membre est obsolète
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
