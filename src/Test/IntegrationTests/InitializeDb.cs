using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class InitializeDb
    {
        private readonly DbContext _db;
        public InitializeDb(DbContext db)
        {
            _db = db;
        }

        public void Init()
        {

        }
    }
}
