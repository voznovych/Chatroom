using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.DAL_Enteties;

namespace DAL
{
    public class DAL : IDAL
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

        public DAL()
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

        #region User table
        public User AddUser(User user)
        {
            var newUser = _ctx.Users.Add(user);
            _ctx.SaveChanges();

            return newUser;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _ctx.Users;
        }

        public bool IsLoginExist(string login)
        {
            return _ctx.Users.FirstOrDefault(u => u.Login == login) != null;
        }

        public User GetUserByLoginAndPassword(string login, string password)
        {
            return _ctx.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public void SetOfflineStatus(int userId)
        {
            _ctx.Users.Find(userId).Status = this.GetOfflineUserStatus();
            _ctx.SaveChanges();
        }
        public void SetDoNotDisturbStatus(int userId)
        {
            _ctx.Users.Find(userId).Status = this.GetDoNotDisturbUserStatus();
            _ctx.SaveChanges();
        }
        public void SetOnlineStatus(int userId)
        {
            _ctx.Users.Find(userId).Status = this.GetOnlineUserStatus();
            _ctx.SaveChanges();
        }
        #endregion

        #region Sex 
        public IQueryable<Sex> GetAllSexes()
        {
            return _ctx.Sexes;
        }

        public Sex GetSex(string name)
        {
            return _ctx.Sexes.FirstOrDefault(s => s.Name == name);
        }
        #endregion

        #region Country table
        public IQueryable<Country> GetAllCountries()
        {
            return _ctx.Countries;
        }
        #endregion

        #region Genre table
        public IQueryable<Genre> GetAllGenres()
        {
            return _ctx.Genres;
        }
        #endregion

        #region UserStatus table
        public IQueryable<UserStatus> GetAllStatuses()
        {
            return _ctx.UserStatuses;
        }

        public UserStatus GetUserStatus(string name)
        {
            return _ctx.UserStatuses.FirstOrDefault(s => s.Name == name);
        }
        public UserStatus GetOfflineUserStatus()
        {
            return GetUserStatus(ChatRoomInitializer.STATUS_OFFLINE);
        }
        public UserStatus GetDoNotDisturbUserStatus()
        {
            return GetUserStatus(ChatRoomInitializer.STATUS_DND);
        }
        public UserStatus GetOnlineUserStatus()
        {
            return GetUserStatus(ChatRoomInitializer.STATUS_ONLINE);
        }
        #endregion
    }
}
