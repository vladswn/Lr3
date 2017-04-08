using System.Collections.Generic;

namespace Project.Models
{
    interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Detail(int id);
        void Create(T create);
        void Delete(int id);
        void Update(T update);
    }
}
