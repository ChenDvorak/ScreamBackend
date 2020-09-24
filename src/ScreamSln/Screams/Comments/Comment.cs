using System;
using System.Collections.Generic;
using System.Text;

namespace Screams.Comments
{
    public class Comment
    {
        /// <summary>
        /// where it from
        /// </summary>
        private readonly ScreamBackend.DB.ScreamDB _db;

        internal ScreamBackend.DB.Tables.Comment Model { get; }

        /// <summary>
        /// scream's state
        /// </summary>
        [Flags]
        public enum Status
        {
            WaitAudit = 0 << 1,
            Passed = 0 << 2,
            Recycle = 0 << 3,
        }
    }
}
