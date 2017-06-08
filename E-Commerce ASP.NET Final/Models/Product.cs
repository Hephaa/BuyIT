namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Amount { get; set; }
    }
}
