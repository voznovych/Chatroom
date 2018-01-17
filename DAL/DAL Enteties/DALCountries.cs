using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    public class DALCountries : IDALCountries
    {
        private readonly ChatRoomContext _ctx;
        public DALCountries(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<Country> GetAll()
        {
            return _ctx.Countries;
        }
    }
}
