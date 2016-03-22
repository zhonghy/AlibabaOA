namespace AlibabaOA.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class StudentEntities : DbContext
    {     
        public StudentEntities()
            : base("name=StudentEntities")
        {
            Database.SetInitializer<StudentEntities>(null);
        }

        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Chapter3.Student");
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Degree { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}