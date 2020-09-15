using System;

namespace Infrastructures
{
    /// <summary>
    /// paging
    /// </summary>
    public abstract class Paging
    {
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
        public int TotalPage { get; set; }
    }
}
