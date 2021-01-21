using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("rating", Schema = "public")]
    public class Rating
    {
        [Column("id")] public long? Id { get; set; }
        [Column("rate")] public int? Rate { get; set; }
        [Column("message")] public string Message { get; set; }
        [Column("product_id")] public long? Product { get; set; }
        [Column("account_id")] public long? Account { get; set; }
    }
}