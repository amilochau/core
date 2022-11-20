using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Milochau.Core.Functions.Helpers
{
    /// <summary>Validation problem details</summary>
    public class ValidationProblemDetails : ProblemDetails
    {
        /// <summary>Validation errors</summary>
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);

        /// <summary>Constructor</summary>
        public ValidationProblemDetails()
        {
            Title = "One or more validation errors occurred.";
        }

        /// <summary>Constructor</summary>
        public ValidationProblemDetails(ReadOnlyDictionary<string, Collection<string?>> modelState) : this()
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            foreach (KeyValuePair<string, Collection<string?>> item in modelState)
            {
                string key = item.Key;
                Collection<string?> errors = item.Value;
                if (errors == null || errors.Count <= 0)
                {
                    continue;
                }

                if (errors.Count == 1)
                {
                    string text = GetErrorMessage(errors[0]);
                    Errors.Add(key, new string[1] { text });
                    continue;
                }

                string[] array = new string[errors.Count];
                for (int i = 0; i < errors.Count; i++)
                {
                    array[i] = GetErrorMessage(errors[i]);
                }

                Errors.Add(key, array);


                static string GetErrorMessage(string? error)
                {
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        return error;
                    }

                    return "The input was not valid.";
                }
            }
        }

        /// <summary>Constructor</summary>
        public ValidationProblemDetails(IDictionary<string, string[]> errors)  : this()
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            Errors = new Dictionary<string, string[]>(errors, StringComparer.Ordinal);
        }
    }

    /// <summary>Problem details</summary>
    public class ProblemDetails
    {
        /// <summary>Problem type</summary>
        public string? Type { get; set; }

        /// <summary>Summary of the problem type</summary>
        public string? Title { get; set; }

        /// <summary>HTTP status code</summary>
        public int? Status { get; set; }

        /// <summary>Explanation of the problem</summary>
        public string? Detail { get; set; }
    }
}
