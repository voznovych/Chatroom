using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.DAL_Enteties;

namespace DAL
{
    public class DALClass : IDAL
    {
        private readonly ChatRoomContext _ctx;

        public IDALCountries Countries { get; }
        public IDALGenres Genres { get ; }
        public IDALMessages Messages { get ; }
        public IDALRooms Rooms { get ; }
        public IDALSexes Sexes { get ; }
        public IDALUsers Users { get ; }
        public IDALUserStatuses UserStatuses { get ; }
        public IDALVisitInfos VisitInfos { get ; }

        public DALClass()
        {
            _ctx = new ChatRoomContext();
            Countries = new DALCountries(_ctx);
            Genres = new DALGenres(_ctx);
            Messages = new DALMessages(_ctx);
            Rooms = new DALRooms(_ctx);
            Sexes = new DALSexes(_ctx);
            Users = new DALUsers(_ctx);
            UserStatuses = new DALUserStatuses(_ctx);
            VisitInfos = new DALVisitInfos(_ctx);
        }
    }
}
