namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OrderModel : DbContext
    {
        public OrderModel()
            : base("name=OrderModel2")
        {
        }

        public virtual DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
