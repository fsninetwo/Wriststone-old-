using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("order", Schema = "public")]
    public class Order
    {
        [Column("id")] public long Id { get; set; }
        [Column("date")] public DateTime Date { get; set; }
        [Column("account_id")] public long Account { get; set; }
        [Column("payment")] public string Payment { get; set; }
        [Column("price")] public decimal Price { get; set; }
    }
}