using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDAL
    {
        IDALCountries Countries { get; }
        IDALGenres Genres { get; }
        IDALMessages Messages { get; }
        IDALRooms Rooms { get; }
        IDALSexes Sexes { get; }
        IDALUsers Users { get; }
        IDALUserStatuses UserStatuses { get; }
        IDALVisitInfos VisitInfos { get; }
    }
}
