namespace Milochau.Core.Abstractions.Models.Apis
{
    /// <summary>Parsable from query</summary>
    public interface IQueryParsable
    {
        /// <summary>Try parse from a query</summary>
        /// <returns>Parse success</returns>
        bool TryParse(string query);
    }
}
