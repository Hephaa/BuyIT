namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }
    }
}
