using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("product", Schema = "public")]
    public class Product
    {
        [Column("id")] public long Id { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("description")] public string Description { get; set; }
        [Column("price")] public decimal Price { get; set; }
        [Column("publisher")] public string Publisher { get; set; }
        [Column("developer")] public string Developer { get; set; }
        [Column("category_id")] public long Category { get; set; }
    }
}