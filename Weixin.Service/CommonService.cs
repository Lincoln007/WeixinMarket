using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service
{
    public class CommonService<T> where T:BaseEntity
    {
        private WeixinDbContext db;
        public CommonService(WeixinDbContext db)
        {
            this.db = db;
        }

        public IQueryable<T> GetAll()
        {
            return db.Set<T>().Where(a => a.IsDeleted == false);
        }

        public async Task<long> TotalCount()
        {
            return await db.Set<T>().LongCountAsync();
        }

        //分页数据
        public IQueryable<T> GetPagedData(int startIndex,int count)
        {
            return GetAll().OrderByDescending(a => a.Id).Skip(startIndex).Take(count);
        }

        public async Task<T> GetById(long id)
        {
            return await GetAll().Where(a => a.Id == id).SingleOrDefaultAsync();
        }

        public async Task Delete(long id)
        {
            var data = await GetById(id);
            data.IsDeleted = true;
            await db.SaveChangesAsync();
        }
    }
}
