namespace Milochau.Core.Functions.Helpers
{
    /// <summary>Validation result</summary>
    /// <typeparam name="TRequestData">Type of request data parsed from body</typeparam>
    public class ValidationResult<TRequestData>
    {
        /// <summary>Data</summary>
        /// <remarks>@Nullable</remarks>
        public TRequestData Data { get; set; }

        /// <summary>Whether request is valid</summary>
        public bool IsValid { get; set; }

        /// <summary>Validation problem details</summary>
        /// <remarks>@Nullable</remarks>
        public ValidationProblemDetails ProblemDetails { get; set; }
    }
}
