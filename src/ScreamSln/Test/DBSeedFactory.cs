using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Test
{
    public class DBSeedFactory: IDisposable
    {
        protected ScreamBackend.DB.ScreamDB _db;
        private bool disposedValue;

        public DBSeedFactory()
        {

            SeedInit();
        }

        private void SeedInit()
        {
            _db = new ScreamBackend.DB.ScreamDB(
                new DbContextOptionsBuilder<ScreamBackend.DB.ScreamDB>()
                    .UseInMemoryDatabase("scream").Options
            );
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~DBSeedFactory()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
