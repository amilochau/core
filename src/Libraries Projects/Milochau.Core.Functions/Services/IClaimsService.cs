using Microsoft.Azure.Functions.Worker.Http;
using Milochau.Core.Functions.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Services
{
    /// <summary>Claims service</summary>
    public interface IClaimsService
    {
        /// <summary>Get current user from a HTTP request</summary>
        IdentityUser GetUser(HttpRequestData request);
    }
}
