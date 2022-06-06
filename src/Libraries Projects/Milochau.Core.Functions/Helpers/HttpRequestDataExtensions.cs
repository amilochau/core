using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Core.Functions.Helpers
{
    /// <summary>Extension methods for <see cref="HttpRequestData"/></summary>
    public static class HttpRequestDataExtensions
    {
        /// <summary>Read and parse data from JSON body, validate content</summary>
        /// <typeparam name="TRequestData">Type of request data</typeparam>
        public static async Task<ValidationResult<TRequestData>> ReadAndValidateRequestDataAsync<TRequestData>(this HttpRequestData request, CancellationToken cancellationToken)
        {
            var validationResult = new ValidationResult<TRequestData>();
            var validationResults = new List<ValidationResult>();

            validationResult.Data = await request.ReadFromJsonAsync<TRequestData>(cancellationToken);
            if (validationResult.Data != null)
            {
                validationResult.IsValid = Validator.TryValidateObject(validationResult.Data, new ValidationContext(validationResult.Data), validationResults, true);
            }

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var result in validationResults)
                {
                    if (result.ErrorMessage == null)
                        continue;

                    foreach (var memberName in result.MemberNames)
                    {
                        modelStateDictionary.AddModelError(memberName, result.ErrorMessage);
                    }
                }
                validationResult.ProblemDetails = new ValidationProblemDetails(modelStateDictionary);
            }

            return validationResult;
        }

        /// <summary>Read and parse data from query, validate content</summary>
        /// <typeparam name="TRequestData">Type of request data</typeparam>
        public static Task<ValidationResult<TRequestData>> ReadAndValidateRequestQueryAsync<TRequestData>(this HttpRequestData request, CancellationToken cancellationToken)
            where TRequestData : IQueryParsable, new()
        {
            var validationResult = new ValidationResult<TRequestData>();
            var validationResults = new List<ValidationResult>();

            validationResult.Data = new TRequestData();
            validationResult.Data.TryParse(request.Url.Query);
            validationResult.IsValid = Validator.TryValidateObject(validationResult.Data, new ValidationContext(validationResult.Data), validationResults, true);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (var result in validationResults)
                {
                    if (result.ErrorMessage == null)
                        continue;

                    foreach (var memberName in result.MemberNames)
                    {
                        modelStateDictionary.AddModelError(memberName, result.ErrorMessage);
                    }
                }
                validationResult.ProblemDetails = new ValidationProblemDetails(modelStateDictionary);
            }

            return Task.FromResult(validationResult);
        }

        /// <summary>Write response as JSON</summary>
        /// <typeparam name="TResponseData">Type of response data</typeparam>
        public static async Task<HttpResponseData> WriteResponseAsJsonAsync<TResponseData>(this HttpRequestData request, TResponseData responseData, HttpStatusCode statusCode, CancellationToken cancellationToken)
        {
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(responseData, statusCode, cancellationToken);
            return response;
        }

        /// <summary>Write empty response</summary>
        public static HttpResponseData WriteEmptyResponseAsync(this HttpRequestData request, HttpStatusCode statusCode)
        {
            var response = request.CreateResponse();
            response.StatusCode = statusCode;
            return response;
        }
    }
}
