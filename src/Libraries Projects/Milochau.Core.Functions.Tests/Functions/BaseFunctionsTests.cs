using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Milochau.Core.Functions.Tests.Functions
{
    public abstract class BaseFunctionsTests
    {
        protected HttpRequestData CreateHttpRequestData(string method, string path)
            => CreateHttpRequestData(method, path, default);

        protected HttpRequestData CreateHttpRequestData(string method, string path, QueryString query)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var context = new Mock<FunctionContext>();
            context.SetupProperty(c => c.InstanceServices, serviceProvider);
            var httpRequestData = new Mock<HttpRequestData>(context.Object);
            var httpResponseData = new Mock<HttpResponseData>(context.Object);

            var uriBuilder = new UriBuilder
            {
                Path = path,
                Query = query.Value
            };

            httpRequestData.SetupGet(x => x.Method).Returns(method);
            httpRequestData.SetupGet(x => x.Url).Returns(uriBuilder.Uri);
            httpRequestData.SetupGet(x => x.Body).Returns(new MemoryStream());

            httpRequestData.Setup(x => x.CreateResponse()).Returns(httpResponseData.Object);
            httpResponseData.SetupProperty(r => r.Headers, new HttpHeadersCollection());
            httpResponseData.SetupProperty(r => r.StatusCode);
            httpResponseData.SetupProperty(r => r.Body, new MemoryStream());

            return httpRequestData.Object;
        }

        protected string GetResponseAsText(HttpResponseData httpResponseData, HttpStatusCode expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, httpResponseData.StatusCode);
            Assert.IsNotNull(httpResponseData.Body);
            httpResponseData.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpResponseData.Body);

            return reader.ReadToEnd();
        }

        protected TResponse GetResponseAsJson<TResponse>(HttpResponseData httpResponseData, HttpStatusCode expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, httpResponseData.StatusCode);
            Assert.IsNotNull(httpResponseData.Body);
            httpResponseData.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpResponseData.Body);

            var response = reader.ReadToEnd();
            Assert.IsNotNull(response);
            return JsonConvert.DeserializeObject<TResponse>(response);
        }
    }
}
