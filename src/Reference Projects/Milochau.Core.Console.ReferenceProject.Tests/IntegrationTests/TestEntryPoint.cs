using System;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Console.ReferenceProject.Tests
{
    public partial class EntryPointRegistrationTests
    {
        public class TestEntryPoint : CoreConsoleEntryPoint
        {
            private readonly IBusinessService businessService;

            public TestEntryPoint(IBusinessService businessService)
            {
                this.businessService = businessService;
            }

            public override Task<int> RunAsync(CancellationToken cancellationToken)
            {
                businessService.Call();
                return Task.FromResult(0);
            }
        }
    }
}
