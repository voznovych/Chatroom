using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDALVisitInfos
    {
        IQueryable<VisitInfo> GetAll();
        void Add(VisitInfo info);
        void Update(int id, DateTime date);
    }
}
