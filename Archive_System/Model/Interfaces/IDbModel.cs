using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive_System.Model.Interfaces
{
    public interface IDbModel<T>
    {
        public static abstract IEnumerable<T> GetAll(Func<T, bool> selector); 
        public static abstract T GetById(int id); 
        public static abstract T Create(T item); 
        public static abstract T Update(T item); 
        public static abstract bool Remove(T item); 
    }
}
