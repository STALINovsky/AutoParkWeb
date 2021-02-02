using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoParkData.Model.Orders;
using AutoParkData.Repositories.Base;

namespace AutoParkData.Repositories.Interfaces
{
    public interface ISparePartRepository : IRepository<SparePart>
    {
        Task<IEnumerable<SparePart>> GetSpareParts();

    }
}
