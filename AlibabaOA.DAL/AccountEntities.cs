namespace AlibabaOA.DAL
{
    using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

    public class AccountEntities : DbContext
    {
        public AccountEntities()
            : base("name=AccountEntities")
        {
            Database.SetInitializer<AccountEntities>(null);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Chapter3.Account");
            modelBuilder.Entity<Order>().ToTable("Chapter3.Order");

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int AccountId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public Decimal Amount { get; set; }
        public int AccountId { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public virtual Account Account { get; set; }
    }
}