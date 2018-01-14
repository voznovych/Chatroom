using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL;

namespace DAL.DAL_Enteties
{
    class DALGenres : IDALGenres
    {
        private readonly ChatRoomContext _ctx;
        public DALGenres(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<Genre> GetAll()
        {
            return _ctx.Genres;
        }
    }
}
