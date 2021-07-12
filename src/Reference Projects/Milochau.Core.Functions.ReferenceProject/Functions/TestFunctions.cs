using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Milochau.Core.Models;
using Milochau.Core.Abstractions;

namespace Milochau.Core.Functions.ReferenceProject
{
    public class TestFunctions
    {
        private readonly CoreHostOptions coreHostOptions;
        private readonly IApplicationHostEnvironment applicationHostEnvironment;

        public TestFunctions(IOptions<CoreHostOptions> coreHostOptions,
            IApplicationHostEnvironment applicationHostEnvironment)
        {
            this.coreHostOptions = coreHostOptions.Value;
            this.applicationHostEnvironment = applicationHostEnvironment;
        }

        [FunctionName("CoreHostOptions")]
        public IActionResult RunCoreHostOptions([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "CoreHostOptions")] HttpRequest req)
        {
            return new OkObjectResult(coreHostOptions);
        }

        [FunctionName("ApplicationHostEnvironment")]
        public IActionResult RunApplicationHostEnvironment([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ApplicationHostEnvironment")] HttpRequest req)
        {
            return new OkObjectResult(applicationHostEnvironment);
        }
    }
}
