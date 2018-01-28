using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    class DALVisitInfos : IDALVisitInfos
    {
        ChatRoomContext _ctx;

        public DALVisitInfos(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }
        public void Add(VisitInfo info)
        {
            _ctx.VisitInfos.Add(info);
            _ctx.SaveChanges();
        }

        public IQueryable<VisitInfo> GetAll()
        {
            return _ctx.VisitInfos;
        }

        public void Update(int id, DateTime date)
        {
            VisitInfo temp = _ctx.VisitInfos.FirstOrDefault(v => v.Id == id);

            temp.LastDateOfVisit = date;

            _ctx.SaveChanges();
        }
    }
}
