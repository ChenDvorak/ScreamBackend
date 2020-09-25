using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Screams.Comments
{
    /// <summary>
    /// Comment
    /// </summary>
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

        internal Comment(ScreamBackend.DB.Tables.Comment model, ScreamBackend.DB.ScreamDB db)
        {
            Model = model;
            _db = db;
        }

        /// <summary>
        /// Increase comment hidden count
        /// </summary>
        /// <returns></returns>
        public async Task<int> IncreaseHidden()
        {
            Model.HiddenCount++;
            _db.Comments.Update(Model);
            int effect = await _db.SaveChangesAsync();
            if (effect == 1)
                return Model.HiddenCount;
            throw new Exception("Increase comment hidden count fail");
        }
    }
}
