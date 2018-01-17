using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    class DALMessages : IDALMessages
    {
        ChatRoomContext _ctx;
        public DALMessages(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }
        public void Add(Message msg)
        {
            _ctx.Messages.Add(msg);
            _ctx.SaveChanges();
        }

        public IQueryable<Message> GetAll()
        {
            return _ctx.Messages;
        }
    }
}
