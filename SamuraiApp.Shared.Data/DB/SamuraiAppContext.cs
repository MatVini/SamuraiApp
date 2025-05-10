using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp_Console;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.Shared.Data.DB
{
    public class SamuraiAppContext : DbContext
    {
        public DbSet<Samurai> Samurai { get; set; }
        private string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SamuraiApp_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
