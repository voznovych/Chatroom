using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALMessages
    {
        IQueryable<Message> GetAll();
        void Add(Message msg);
    }
}
