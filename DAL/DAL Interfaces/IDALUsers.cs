using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALUsers
    {
        void Add(User user);
        void Update(User user);
        IQueryable<User> GetAll();
    }
}
