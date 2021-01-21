using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wriststone.Models.Table;

namespace Wriststone.Models.ViewModel
{
    public class AccountPostModel
    {
        public Account Account { get; set; }
        public Post Post { get; set; }
    }
}