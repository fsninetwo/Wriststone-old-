using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("post", Schema = "public")]
    public class Post
    {
        [Column("id")] public long Id { get; set; }
        [Column("context")] public string Context { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("thread_id")] public long Thread { get; set; }
        [Column("account_id")] public long Account { get; set; }
        [Column("status_id")] public long? Status { get; set; }
    }
}