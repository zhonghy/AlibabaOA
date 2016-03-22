using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibabaOA.DAL
{
    public class BusinessContext : DbContext
    {
        public DbSet<Business> Businesses { get; set; }

        public BusinessContext()
            : base("name=ProductEFContext")
        {
            Database.SetInitializer<BusinessContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
