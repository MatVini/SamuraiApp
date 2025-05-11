using SamuraiApp_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Shared.Data.DB
{
    public class DojoDAL
    {
        private readonly SamuraiAppContext Context;

        public DojoDAL(SamuraiAppContext context)
        {
            Context = context;
        }

        public void Create(Dojo dj)
        {
            Context.Dojo.Add(dj);
            Context.SaveChanges();
        }

        public IEnumerable<Dojo> Read()
        {
            return Context.Dojo.ToList();
        }

        public Dojo? ReadByName(string name)
        {
            return Context.Dojo.FirstOrDefault(x => x.Name == name);
        }

        public void Update(Dojo dj)
        {
            Context.Dojo.Update(dj);
            Context.SaveChanges();
        }

        public void Delete(Dojo dj)
        {
            Context.Dojo.Remove(dj);
            Context.SaveChanges();
        }
    }
}
