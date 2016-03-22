using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibabaOA.DAL
{
    public class AssociateEntities: DbContext
    {
        public AssociateEntities()
            : base("name=ProductEFContext")
        {
            Database.SetInitializer<AssociateEntities>(null);
        }

        public DbSet<Associate> Associates { get; set; }
        public DbSet<AssociateSalary> AssociateSalaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Associate>().ToTable("Chapter3.Associate");
            modelBuilder.Entity<AssociateSalary>().ToTable("Chapter3.AssociateSalary");
            modelBuilder.Entity<AssociateSalary>().HasKey(x => x.SalaryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
