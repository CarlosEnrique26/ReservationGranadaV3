using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class DBContext : DbContext
    {
        public DbSet<TbChair> TbChair { get; set; }
        public DbSet<TbReservation> TbReservation { get; set; }
        public DbSet<TbTable> TbTable { get; set; }
        public DbSet<TbUser> TbUser { get; set; }
        public DbSet<TbConfig> TbConfig { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}