using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milochau.Core.Abstractions.Models.Apis
{
    /// <summary>Request to get a list</summary>
    /// <typeparam name="TSearchKeys">Search keys supported</typeparam>
    /// <typeparam name="TOrderTypes">Order types supported</typeparam>
    public class ListRequest<TSearchKeys, TOrderTypes> : IQueryParsable
        where TSearchKeys : notnull
        where TOrderTypes : struct
    {
        private const int maxRows = 100;

        /// <summary>Rows</summary>
        [Required]
        [Range(1, maxRows)]
        public int Rows { get; set; }

        /// <summary>Search</summary>
        public IDictionary<TSearchKeys, string> Search { get; set; } = new Dictionary<TSearchKeys, string>();

        /// <summary>Order type</summary>
        public TOrderTypes OrderType { get; set; }

        /// <summary>First id</summary>
        public string? FirstId { get; set; }

        /// <summary>Last id</summary>
        public string? LastId { get; set; }

        /// <summary>Try parse the current model from a query</summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool TryParse(string query)
        {
            var queryString = HttpUtility.ParseQueryString(query);

            if (!int.TryParse(queryString.GetValues("rows")?.FirstOrDefault(), out int rows))
                return false;

            var searchKeys = Enum.GetValues(typeof(TSearchKeys)).Cast<TSearchKeys>();
            foreach (TSearchKeys searchKey in searchKeys)
            {
                var searchKeyDescription = typeof(TSearchKeys).GetField($"{searchKey}")?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                if (searchKeyDescription == null)
                    continue;

                var searchValue = queryString.GetValues($"search[{searchKeyDescription.Description}]")?.FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Search.Add(searchKey, searchValue);
                }
            }

            Rows = rows;
            FirstId = queryString.GetValues("firstId")?.FirstOrDefault();
            LastId = queryString.GetValues("lastId")?.FirstOrDefault();

            return true;
        }
    }

}
