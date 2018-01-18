using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    public class DALSexes : IDALSexes
    {
        ChatRoomContext _ctx;

        public DALSexes(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Sex> GetAll()
        {
            return _ctx.Sexes;
        }
    }
}
