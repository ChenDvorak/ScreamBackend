using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    /// <summary>
    /// 数据库种子测试类
    /// 测试类继承此类，在实例化时就会创建一个用于测试的数据库
    /// </summary>
    public abstract class DatabeseSeedTest
    {
        protected DbContextOptions<ScreamBackend.DB.ScreamDB> contextOptions { get; }
        public DatabeseSeedTest(DbContextOptions<ScreamBackend.DB.ScreamDB> options)
        {
            contextOptions = options;
            Seed();
        }

        private void Seed()
        {
            using var context = new ScreamBackend.DB.ScreamDB(contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


        }
    }
}
