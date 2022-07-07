﻿using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Milochau.Core.Abstractions;
using Milochau.Core.Functions.Services.Models;
using Milochau.Core.Infrastructure.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Milochau.Core.Functions.Services.Implementations
{
    /// <summary>Claims service</summary>
    public class ClaimsService : IClaimsService
    {
        private readonly IApplicationHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;

        /// <summary>Constructor</summary>
        public ClaimsService(IApplicationHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
        }

        /// <summary>Get current user from a HTTP request</summary>
        public IdentityUser GetUser(HttpRequestData request)
        {
            var result = new IdentityUser();

            var aadIdentity = request.Identities.FirstOrDefault(x => x.AuthenticationType != null && x.AuthenticationType.Equals("aad", StringComparison.OrdinalIgnoreCase));
            if (aadIdentity != null)
            {
                result.Id = aadIdentity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                result.Name = aadIdentity.Claims.First(x => x.Type == "name").Value;
                result.Email = aadIdentity.Claims.First(x => x.Type == "emails").Value;
            }
            else if (hostEnvironment.HostName.Equals(ApplicationHostEnvironment.LocalHostName, StringComparison.OrdinalIgnoreCase))
            {
                var queryString = HttpUtility.ParseQueryString(request.Url.Query);

                result.Id = queryString.GetValues("userId")?.FirstOrDefault()
                    ?? configuration["Identity:UserId"]
                    ?? throw new ArgumentException("Please use 'userId' query parameter in local.", nameof(request));
                result.Name = queryString.GetValues("userName")?.FirstOrDefault()
                    ?? configuration["Identity:UserName"]
                    ?? throw new ArgumentException("Please use 'userName' query parameter in local.", nameof(request));
                result.Email = queryString.GetValues("userEmail")?.FirstOrDefault()
                    ?? configuration["Identity:UserEmail"]
                    ?? throw new ArgumentException("Please use 'userEmail' query parameter in local.", nameof(request));
            }

            return result;
        }
    }
}
