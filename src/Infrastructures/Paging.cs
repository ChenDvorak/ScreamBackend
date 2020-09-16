/*
 *  Paging
 *  There is abstract Paging class.
 *  Inherit this which need paging of request parameters or response data
 */

using System;
using System.Collections.Generic;

namespace Infrastructures
{
    /// <summary>
    /// abstract paging
    /// </summary>
    /// <typeparam name="T">type of list item</typeparam>
    public abstract class Paging<T> where T : class
    {
        protected Paging(int index, int size)
        {
            Index = index;
            Size = size;
        }
        protected Paging(int index, int size, int capacity): this(index, size)
        {
            _params = new Dictionary<string, string>(capacity);
        }

        public string this[string key]
        {
            get
            {
                return _params.ContainsKey(key) ? _params[key] : null;
            }
            set => _params[key] = value;
        }

        /// <summary>
        /// current index of page
        /// </summary>
        public int Index { get; set; } = 1;
        /// <summary>
        /// current size of data of page
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// total size of data
        /// </summary>
        public int TotalSize { get; set; }
        /// <summary>
        /// total size of pages
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalSize % Size == 0)
                    return TotalSize / Size;
                return TotalSize / Size + 1;
            }
        }
        /// <summary>
        /// parameters that you need
        /// </summary>
        [NonSerialized, Newtonsoft.Json.JsonIgnore]
        private Dictionary<string, string> _params;
        /// <summary>
        /// skip count of rows
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int Skip => Size * (Index - 1);
        /// <summary>
        /// data
        /// </summary>
        public List<T> List { get; set; }
    }
}
