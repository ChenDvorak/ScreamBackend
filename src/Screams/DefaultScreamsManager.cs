using Infrastructures;
using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screams
{
    public class DefaultScreamsManager : IScreamsManager
    {
        private readonly ScreamDB _db;
        public DefaultScreamsManager(ScreamDB db)
        {
            _db = db;
        }

        public async Task<ScreamResult<int>> PostScreamAsync(Models.NewScreamtion model)
        {
            var screamer = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == model.ScreamerId);
            if (screamer == null)
                return QuickResult.Unsuccessful(0, "作者不存在");

            var newScream = new Scream
            { 
                Screamer = screamer,
                Content = model.Content,
                CreateDate = DateTime.Now
            };
            await _db.Screams.AddAsync(newScream);
            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return QuickResult.Successful(newScream.Id);
            throw new Exception("发布失败");
        }

        public Task<ScreamResult> RemoveAsync(Screamtion screamtion)
        {
            throw new NotImplementedException();
        }

        public async Task<ScreamResult<ScreamPaging>> GetScreamsAsync(Paging<ScreamPaging.ScreamItem> paging)
        {
            paging.List = await _db.Screams.AsNoTracking()
                                           .OrderByDescending(scream => scream.CreateDate)
                                           .Where(s => !s.Hidden)
                                           .Skip(paging.Skip)
                                           .Take(paging.Size)
                                           .Include(s => s.Screamer)
                                           .Select(s => new ScreamPaging.ScreamItem
                                           { 
                                               Id = s.Id,
                                               ScreamerId = s.ScreamerId,
                                               Screamer = s.Screamer.UserName,
                                               Content = s.Content,
                                               DateTime = s.CreateDate.ToShortDateString()
                                           })
                                           .ToListAsync();
            paging.TotalSize = await _db.Screams.CountAsync(s => !s.Hidden);

            return QuickResult.Successful(paging as ScreamPaging); ;
        }
    }
}
