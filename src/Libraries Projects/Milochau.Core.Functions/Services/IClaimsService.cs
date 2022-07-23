using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Functions.Services.Models;

namespace Milochau.Core.Functions.Services
{
    /// <summary>Claims service</summary>
    public interface IClaimsService
    {
        /// <summary>Get current user from a HTTP request</summary>
        IdentityUser? GetUser(HttpRequestData request);
    }
}
