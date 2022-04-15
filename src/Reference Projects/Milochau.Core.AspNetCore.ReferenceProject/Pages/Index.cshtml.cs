using Milochau.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Milochau.Core.AspNetCore.Models;
using Microsoft.Extensions.Configuration;

namespace Milochau.Core.AspNetCore.ReferenceProject.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(IOptions<CoreHostOptions> coreHostOptions,
            IOptions<CoreServicesOptions> coreServicesOptions,
            IConfiguration configuration)
        {
            CoreHostOptions = coreHostOptions.Value;
            CoreServicesOptions = coreServicesOptions.Value;
            Configuration = configuration;
        }

        public CoreHostOptions CoreHostOptions { get; }
        public CoreServicesOptions CoreServicesOptions { get; }
        public IConfiguration Configuration { get; }
    }
}