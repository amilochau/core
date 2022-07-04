using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Milochau.Core.AspNetCore.Tests.TestHelpers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyOptions>
    {
        private const string apiKeyHeader = "X-Api-Key";

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(apiKeyHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
            }

            // Get authorization key
            var authorizationHeader = Request.Headers[apiKeyHeader].ToString();

            if (!authorizationHeader.Equals(Options.ApiKey, System.StringComparison.Ordinal))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key is incorrect."));
            }

            var claimsIdentity = GenerateClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimsIdentity));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }

        private static ClaimsIdentity GenerateClaimsIdentity()
        {
            var claims = new List<Claim>();

            return new ClaimsIdentity(new GenericIdentity("ApiUser"), claims);
        }
    }

    public class ApiKeyOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; } = null!;
    }
}
