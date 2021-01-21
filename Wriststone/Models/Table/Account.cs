using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wriststone.Models.Table
{
    [Table("account", Schema = "public")]
    public class Account
    {
        [Column("id")] public long Id { get; set; }
        [Column("login")] public string Login { get; set; }
        [Column("password")]  public string Password { get; set; }
        [Column("email")] public string Email { get; set; }
        [Column("fullname")] public string Fullname { get; set; }
        [Column("created")] public DateTime Created { get; set; }
        [Column("status_id")] public long? Status { get; set; }
    }
}