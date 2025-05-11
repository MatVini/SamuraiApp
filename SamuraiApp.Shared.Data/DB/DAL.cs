using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Shared.Data.DB
{
    public class DAL<T> where T : class
    {
        private readonly SamuraiAppContext Context;

        public DAL()
        {
            Context = new SamuraiAppContext();
        }

        public void Create(T value)
        {
            Context.Set<T>().Add(value);
            Context.SaveChanges();
        }

        public IEnumerable<T> Read() => Context.Set<T>().ToList();

        public void Update(T value)
        {
            Context.Set<T>().Update(value);
            Context.SaveChanges();
        }

        public void Delete(T value)
        {
            Context.Set<T>().Remove(value);
            Context.SaveChanges();
        }

        public T? ReadBy(Func<T, bool> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }
    }
}
