namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CompanyModel : DbContext
    {
        public CompanyModel()
            : base("name=CompanyModel")
        {
        }

        public virtual DbSet<Company> Company { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
