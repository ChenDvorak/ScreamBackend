using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Test
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
        /// The Faker User to test
        /// </summary>
        protected ScreamBackend.DB.Tables.User FakeUser;
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
                UserName = "Dvorak",
                NormalizedUserName = "DVORAK",
                Email = "dvorak@outlook.com",
                NormalizedEmail = "DVORAK@OUTLOOK.COM",
                IsAdmin = false,
                CreateDateTime = DateTime.Now,
                Avatar = ""
            };

            if (!context.Users.Any())
                context.Users.Add(FakeUser);
            if (!context.Screams.Any())
                context.Screams.AddRange(FakerScreamModels);

            int _ = context.SaveChanges();
        }

        /// <summary>
        /// size 12
        /// </summary>
        protected List<ScreamBackend.DB.Tables.Scream> FakerScreamModels => new List<ScreamBackend.DB.Tables.Scream>
        {
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_1",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_2",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_3",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_4",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_5",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_6",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_7",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_8",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_9",
                ContentLength = 10
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_10",
                ContentLength = 11
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_11",
                ContentLength = 11,
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakeUser,
                Content = "TEST: SCREAM ITEM_12",
                ContentLength = 11
            },
        };

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
