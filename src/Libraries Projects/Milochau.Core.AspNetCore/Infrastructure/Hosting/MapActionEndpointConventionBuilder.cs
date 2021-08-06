using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;

namespace Milochau.Core.AspNetCore.Infrastructure.Hosting
{
    /// <summary>
    /// Builds conventions that will be used for customization of MapAction <see cref="EndpointBuilder"/> instances.
    /// </summary>
    internal sealed class MapActionEndpointConventionBuilder : IEndpointConventionBuilder
    {
        private readonly IEnumerable<IEndpointConventionBuilder> endpointConventionBuilders;

        internal MapActionEndpointConventionBuilder(IEndpointConventionBuilder endpointConventionBuilder)
        {
            endpointConventionBuilders = new List<IEndpointConventionBuilder> { endpointConventionBuilder };
        }

        internal MapActionEndpointConventionBuilder(params IEndpointConventionBuilder[] endpointConventionBuilders)
        {
            this.endpointConventionBuilders = endpointConventionBuilders;
        }

        /// <summary>
        /// Adds the specified convention to the builder. Conventions are used to customize <see cref="EndpointBuilder"/> instances.
        /// </summary>
        /// <param name="convention">The convention to add to the builder.</param>
        public void Add(Action<EndpointBuilder> convention)
        {
            foreach (var endpointConventionBuilder in endpointConventionBuilders)
            {
                endpointConventionBuilder.Add(convention);
            }
        }
    }
}
