using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    public class DALRooms : IDALRooms
    {
        ChatRoomContext _ctx;
        public DALRooms(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }
        public void Add(Room room)
        {
            _ctx.Rooms.Add(room);
            _ctx.SaveChanges();
        }

        public IQueryable<Room> GetAll()
        {
            return _ctx.Rooms;
        }

        public void Update(Room room)
        {
            var temp = _ctx.Rooms.FirstOrDefault(r => r.Id == room.Id);
            temp.CreatorId = room.CreatorId;
            temp.Description = room.Description;
            temp.GenreId = room.GenreId;
            temp.IsPersonal = room.IsPersonal;
            temp.IsPrivate = room.IsPrivate;
            temp.Name = room.Name;
            temp.Photo = room.Photo;
            // _ctx.Entry(temp).State = System.Data.Entity.EntityState.Modified;
            _ctx.SaveChanges();
        }
    }
}
