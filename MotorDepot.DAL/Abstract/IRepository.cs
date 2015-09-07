using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(Int32 id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Remove(Int32 id);
        void Update(T item);

    }
}
