using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AccountsTest
{
    /// <summary>
    /// Base class of initialize database both InMemory and Redis
    /// </summary>
    public abstract class DBSeedFactory : IDisposable
    {
        private DbConnection _connection;
        private bool disposedValue;
        /// <summary>
        /// InMemor Database
        /// </summary>
        protected readonly ScreamBackend.DB.ScreamDB _db;
        /// <summary>
        /// The Faker Client to test
        /// </summary>
        protected ScreamBackend.DB.Tables.User FakeUser;
        /// <summary>
        /// The Fake Admin to test
        /// </summary>
        protected ScreamBackend.DB.Tables.User FakeAdmin;
        /// <summary>
        /// Redis
        /// </summary>
        protected readonly StackExchange.Redis.ConnectionMultiplexer redisConn;
        protected readonly DbContextOptions contextOptions = new DbContextOptionsBuilder<ScreamBackend.DB.ScreamDB>()
                    .UseSqlite(CreateInMemorDatabase())
                    .Options;
        public DBSeedFactory()
        {
            redisConn = StackExchange.Redis.ConnectionMultiplexer.Connect("localhost");
            _connection = RelationalOptionsExtension.Extract(contextOptions).Connection;
            _connection.Open();
            _db = new ScreamBackend.DB.ScreamDB(contextOptions);
            SeedInit();
        }
        private static DbConnection CreateInMemorDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
        /// <summary>
        /// Initial InMemory database
        /// There will create a faker user in database
        /// </summary>
        protected void SeedInit()
        {
            using var context = new ScreamBackend.DB.ScreamDB(contextOptions);

            context.Database.EnsureCreated();

            FakeUser = new ScreamBackend.DB.Tables.User
            {
                Username = "default",
                NormalizedUsername = "DEFAULT",
                Email = "default@outlook.com",
                NormalizedEmail = "DEFAULT@OUTLOOK.COM",
                IsAdmin = false,
                CreateDateTime = DateTime.Now,
                Avatar = "",
                PasswordHash = "96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e"
            };

            if (!context.Users.Any(u => !u.IsAdmin))
                context.Users.Add(FakeUser);

            _ = context.SaveChanges();
        }

        protected void CreateAdministartor()
        {
            using var context = new ScreamBackend.DB.ScreamDB(contextOptions);

            FakeAdmin = new ScreamBackend.DB.Tables.User
            {
                Username = "Administartor",
                NormalizedUsername = "ADMINISTARTOR",
                Email = "Administartor@outlook.com",
                NormalizedEmail = "ADMINISTARTOR@OUTLOOK.COM",
                IsAdmin = true,
                CreateDateTime = DateTime.Now,
                Avatar = "",
                PasswordHash = "96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e",
            };

            if (!context.Users.Any(u => u.IsAdmin))
                context.Users.Add(FakeAdmin);

            _ = context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }
                _connection.Dispose();
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
