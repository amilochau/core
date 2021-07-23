using Milochau.Core.Abstractions;
using Milochau.Core.AspNetCore.ReferenceProject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;
using Milochau.Core.AspNetCore.Models;

namespace Milochau.Core.AspNetCore.ReferenceProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFeatureManager featureManager;

        public IndexModel(IOptionsSnapshot<TestOptions> testOptions,
            IOptions<CoreHostOptions> coreHostOptions,
            IOptions<CoreServicesOptions> coreServicesOptions,
            IFeatureManager featureManager)
        {
            TestOptions = testOptions.Value;
            CoreHostOptions = coreHostOptions.Value;
            CoreServicesOptions = coreServicesOptions.Value;
            this.featureManager = featureManager;
        }

        public TestOptions TestOptions { get; }
        public CoreHostOptions CoreHostOptions { get; }
        public CoreServicesOptions CoreServicesOptions { get; }

        public bool ReferenceProjectTest { get; private set; }

        public async Task OnGetAsync()
        {
            ReferenceProjectTest = await featureManager.IsEnabledAsync(nameof(FeatureFlags.ReferenceProjectTest));
        }
    }
}