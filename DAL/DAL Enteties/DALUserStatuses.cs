using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    class DALUserStatuses : IDALUserStatuses
    {
        ChatRoomContext _ctx;
        public DALUserStatuses(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<UserStatus> GetAll()
        {
            return _ctx.UserStatuses;
        }
    }
}
