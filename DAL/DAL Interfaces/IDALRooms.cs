using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALRooms
    {
        IQueryable<Room> GetAll();
        void Add(Room room);
        void Update(Room room);
    }
}
