using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wriststone.Models.Table;

namespace Wriststone.Models.ViewModel
{
    public class ThreadPostsModel
    {
        public Account Creator { get; set; }
        public Thread Thread { get; set; }
        public long Post { get; set; }
        public IEnumerable<AccountPostModel> Posts { get; set; }
    }
}