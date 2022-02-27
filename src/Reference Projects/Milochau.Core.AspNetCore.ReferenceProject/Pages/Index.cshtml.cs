using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.ReferenceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Milochau.Core.AspNetCore.Models;

namespace Milochau.Core.AspNetCore.ReferenceProject.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IOptionsSnapshot<TestOptions> testOptions,
            IOptions<CoreHostOptions> coreHostOptions,
            IOptions<CoreServicesOptions> coreServicesOptions)
        {
            TestOptions = testOptions.Value;
            CoreHostOptions = coreHostOptions.Value;
            CoreServicesOptions = coreServicesOptions.Value;
        }

        public TestOptions TestOptions { get; }
        public CoreHostOptions CoreHostOptions { get; }
        public CoreServicesOptions CoreServicesOptions { get; }
    }
}