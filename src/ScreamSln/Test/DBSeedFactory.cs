using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Test
{
    /// <summary>
    /// Base class of initialize database both InMemory and Redis
    /// </summary>
    public abstract class DBSeedFactory : IDisposable
    {
        private bool disposedValue;
        /// <summary>
        /// InMemor Database
        /// </summary>
        protected ScreamBackend.DB.ScreamDB _db;
        /// <summary>
        /// The Faker User to test
        /// </summary>
        protected ScreamBackend.DB.Tables.User FakerUser;
        /// <summary>
        /// Redis
        /// </summary>
        protected readonly StackExchange.Redis.ConnectionMultiplexer redisConn;

        public DBSeedFactory()
        {
            redisConn = StackExchange.Redis.ConnectionMultiplexer.Connect("localhost");
            SeedInit();
        }
        /// <summary>
        /// Initial InMemory database
        /// There will create a faker user in database
        /// </summary>
        private void SeedInit()
        {
            _db = new ScreamBackend.DB.ScreamDB(
                new DbContextOptionsBuilder<ScreamBackend.DB.ScreamDB>()
                    .UseInMemoryDatabase("scream").Options
            );

            FakerUser = new ScreamBackend.DB.Tables.User
            {
                UserName = "Dvorak",
                NormalizedUserName = "DVORAK",
                Email = "dvorak@outlook.com",
                NormalizedEmail = "DVORAK@OUTLOOK.COM",
                IsAdmin = false,
                CreateDateTime = DateTime.Now,
                Avatar = ""
            };

            _db.Users.Add(FakerUser);

            _db.Screams.AddRange(ScreamModels);

            int effects = _db.SaveChanges();
            if (effects != (1 + ScreamModels.Count))
                throw new Exception("Initializ user fail");
        }

        private List<ScreamBackend.DB.Tables.Scream> ScreamModels => new List<ScreamBackend.DB.Tables.Scream>
        { 
            new ScreamBackend.DB.Tables.Scream
            { 
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_1"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_2"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_3"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_4"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_5"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_6"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_7"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_8"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_9"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_10"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_11"
            },
            new ScreamBackend.DB.Tables.Scream
            {
                Author = FakerUser,
                Content = "TEST: SCREAM ITEM_12"
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
