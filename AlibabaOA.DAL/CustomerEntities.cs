namespace AlibabaOA.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CustomerEntities : DbContext
    {
        public CustomerEntities()
            : base("name=StudentEntities")
        {
            Database.SetInitializer<CustomerEntities>(null);
        }

        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Chapter3.Customer");
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}