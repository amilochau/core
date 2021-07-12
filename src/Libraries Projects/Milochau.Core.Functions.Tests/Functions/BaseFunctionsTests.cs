using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Milochau.Core.Functions.Tests.Functions
{
    public abstract class BaseFunctionsTests
    {
        protected HttpContext CreateHttpContext(string method, string path)
            => CreateHttpContext(method, path, default);

        protected HttpContext CreateHttpContext(string method, string path, QueryString query)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = method;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = query;
            httpContext.Response.Body = new MemoryStream();
            return httpContext;
        }

        protected TResponse GetResponseFromActionResult<TResponse>(IActionResult result, int expectedStatusCode)
        {
            var jsonResult = (ObjectResult)result;
            Assert.AreEqual(expectedStatusCode, jsonResult.StatusCode);
            Assert.IsNotNull(jsonResult.Value);

            var response = (TResponse)jsonResult.Value;
            Assert.IsNotNull(response);
            return response;
        }

        protected TResponse GetResponseFromActionResultAsJson<TResponse>(IActionResult result, int expectedStatusCode)
        {
            var jsonResult = (ObjectResult)result;
            Assert.AreEqual(expectedStatusCode, jsonResult.StatusCode);
            Assert.IsNotNull(jsonResult.Value);

            var response = JsonConvert.DeserializeObject<TResponse>(jsonResult.Value.ToString());
            Assert.IsNotNull(response);
            return response;
        }
    }
}
