using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeHolidayManagement.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        ICollection<T> FindAll();

        T FindById(int Id);

        bool Update(T entity);
        bool Delete(T entity);
        bool Create(T entity);

        bool Exists(int id);

        bool Exists(string name);

        bool Save();
    }
}
