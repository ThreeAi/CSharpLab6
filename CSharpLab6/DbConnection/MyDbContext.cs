using CSharpLab6.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLab6.DbConnection
{
    internal class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visit>()
                .HasKey(ccb => new { ccb.ClientId, ccb.BuildingId, ccb.VisitDate });

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Client)
                .WithMany(c => c.Visits)
                .HasForeignKey(v => v.ClientId);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.ClimbingBuilding)
                .WithMany(b => b.Visits)
                .HasForeignKey(v => v.BuildingId);

            modelBuilder.Entity<Client>()
                .HasKey(c => new { c.ClientId });

            modelBuilder.Entity<ClimbingBuilding>()
                .HasKey(b => new { b.BuildingId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; Database=postgres; Username=postgres; Password=qwerty");
            base.OnConfiguring(optionsBuilder);
        }

        public IEnumerable<Visit> GetVisitsWithClientsAndBuildings()
        {
            return Visits
                .Include(v => v.Client)
                .Include(v => v.ClimbingBuilding);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClimbingBuilding> ClimbingBuildings { get; set; }
        public DbSet<Visit> Visits { get; set; }

    }
}
