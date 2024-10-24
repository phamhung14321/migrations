using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Person.Migrations
{
    public class SchoolContext : DbContext
    {

        public SchoolContext() : base("name=Student")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SchoolContext>());
        }

        public virtual DbSet<Student> Students { get; set; }
    }

    public class Student
    {
        [Key]
        public int MSSV { get; set; }

        public string HoTen { get; set; }
        public int Tuoi { get; set; }
        public string Nganh { get; set; }
    }
}