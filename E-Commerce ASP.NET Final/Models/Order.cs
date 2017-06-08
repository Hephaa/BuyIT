namespace E_Commerce_ASP.NET_Final.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(300)]
        public string Adress { get; set; }

        [Required]
        public string Products { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
    }
}
