using Microsoft.Data.SqlClient;
using SamuraiApp_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Shared.Data.DB
{
    public class SamuraiDAL
    {
        private readonly SamuraiAppContext Context;

        public SamuraiDAL(SamuraiAppContext context)
        {
            Context = context;
        }

        public void Create(Samurai sam)
        {
            Context.Samurai.Add(sam);
            Context.SaveChanges();
        }

        public IEnumerable<Samurai> Read()
        {
            return Context.Samurai.ToList();
        }

        public Samurai? ReadByName(string name)
        {
            return Context.Samurai.FirstOrDefault(x => x.Name==name);
        }

        public void Update(Samurai sam)
        {
            Context.Samurai.Update(sam);
            Context.SaveChanges();
        }

        public void Delete(Samurai sam)
        {
            Context.Samurai.Remove(sam);
            Context.SaveChanges();
        }
    }
}
