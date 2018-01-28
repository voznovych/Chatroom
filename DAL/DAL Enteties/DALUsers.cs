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

        public void Add(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
        }

        public IQueryable<User> GetAll()
        {
            return _ctx.Users;
        }

        public void Update(User newUser)
        {
            var user = _ctx.Users.Find(newUser.Id);

            user.Photo = newUser.Photo;
            user.Login = newUser.Login;
            user.Password = newUser.Password;
            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.BirthDate = newUser.BirthDate;
            user.SexId = newUser.SexId;
            user.CountryId = newUser.CountryId;
            user.StatusId = newUser.StatusId;

            _ctx.SaveChanges();
        }
    }
}
