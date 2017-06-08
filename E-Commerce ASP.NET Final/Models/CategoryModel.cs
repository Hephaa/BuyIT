namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CategoryModel : DbContext
    {
        public CategoryModel()
            : base("name=CategoryModel")
        {
        }

        public virtual DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public System.Data.Entity.DbSet<E_Commerce_ASP.NET_Final.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<E_Commerce_ASP.NET_Final.Models.Product> Products { get; set; }
    }
}
