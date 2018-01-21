using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.DAL_Enteties
{
    class DALUsers : IDALUsers
    {
        ChatRoomContext _ctx;

        public DALUsers(ChatRoomContext ctx)
        {
            _ctx = ctx;
        }

        public void AddUser(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
        }

        public IQueryable<User> GetAll()
        {
            return _ctx.Users;
        }

        public void UpdateUser(User user)
        {
            User temp = _ctx.Users.FirstOrDefault(u=> u.Id == user.Id);
            temp.CountryId = user.CountryId;
            temp.DateOfBirth = user.DateOfBirth;
            temp.Name = user.Name;
            temp.Password = user.Password;
            temp.Photo = user.Photo;
            temp.SexId = user.SexId;
            temp.StatusId = user.StatusId;
            temp.Surname = user.Surname;
            //_ctx.Entry(temp).State = System.Data.Entity.EntityState.Modified;
            _ctx.SaveChanges();
        }
    }
}
