using Milochau.Core.Functions.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milochau.Core.Functions.ReferenceProject.Models
{
    public class ValidationFromQuery : IQueryParsable
    {
        [Required]
        [MinLength(2)]
        public string? Key { get; set; }

        public bool TryParse(string query)
        {
            var queryString = HttpUtility.ParseQueryString(query);
            Key = queryString.GetValues("key")?.FirstOrDefault();
            return true;
        }
    }
}
