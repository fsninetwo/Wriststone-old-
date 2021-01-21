using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("forum_category", Schema = "public")]
    public class ForumCategory
    {
        [Column("id")] public long Id { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("category_id")] public long? Category { get; set; }
    }
}