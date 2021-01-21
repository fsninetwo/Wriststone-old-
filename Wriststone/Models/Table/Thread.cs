using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("thread", Schema = "public")]
    public class Thread
    {
        [Column("id")] public long Id { get; set; }
        [Column("subject")] public string Subject { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("account_id")] public long Account { get; set; }
        [Column("category_id")] public long Category { get; set; }
        [Column("status_id")] public long? Status { get; set; }
    }
}