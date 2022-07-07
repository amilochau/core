using System.Collections.Generic;

namespace Milochau.Core.Abstractions.Models.Apis
{
    /// <summary>Result to get a list</summary>
    /// <typeparam name="TListModel"></typeparam>
    public class ListResult<TListModel>
    {
        /// <summary>Rows</summary>
        public int Rows { get; set; }

        /// <summary>Items</summary>
        public IEnumerable<TListModel> Items { get; set; } = new List<TListModel>();

        /// <summary>End reached</summary>
        public bool EndReached { get; set; }

        /// <summary>Constructor</summary>
        public ListResult()
        {
        }

        /// <summary>Constructor</summary>
        public ListResult(int rows, IEnumerable<TListModel> items, bool endReached)
            : this()
        {
            Rows = rows;
            Items = items;
            EndReached = endReached;
        }
    }
}
