using System;
using System.Collections.Generic;
using System.Text;

namespace Screams
{
    /// <summary>
    /// the subject of scream
    /// </summary>
    public class Scream
    {
        private ScreamBackend.DB.Tables.Scream _scream;

        internal Scream(ScreamBackend.DB.Tables.Scream scream)
        {
            _scream = scream;
        }


    }
}
