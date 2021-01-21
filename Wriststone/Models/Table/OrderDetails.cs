using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("order_details", Schema = "public")]
    public class OrderDetails
    {
        [Column("id")] public long Id { get; set; }
        [Column("order_id")] public long Order { get; set; }
        [Column("product_id")] public long Product { get; set; }
        [Column("price")] public decimal Price { get; set; }
    }
}