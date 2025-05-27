using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp_Model;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Shared.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SamuraiApp.Shared.Data.DB
{
    public class SamuraiAppContext :
        IdentityDbContext<AccessUser, AccessRole, int>
    {
        public DbSet<Samurai> Samurai { get; set; }
        public DbSet<Dojo> Dojo { get; set; }
        public DbSet<Kenjutsu> Kenjutsu { get; set; }

        private string ConnectionString = "Server=tcp:samuraiappserver.database.windows.net,1433;Initial Catalog=SamuraiApp_DB_V1;Persist Security Info=False;User ID=mvcs@mail.com@samuraiappserver;Password=On!gash1M4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(ConnectionString)
                .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Samurai>()
                .HasMany(s=>s.KenCollect)
                .WithMany(k=>k.SamCollect);
        }
    }
}
