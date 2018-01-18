using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALUsers
    {
        void AddUser(User user);
        void UpdateUser(User user);
        IQueryable<User> GetAll();
    }
}
