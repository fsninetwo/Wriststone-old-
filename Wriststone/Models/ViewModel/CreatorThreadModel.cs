using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wriststone.Models.Table;

namespace Wriststone.Models.ViewModel
{
    public class CreatorThreadModel
    {
        public Thread Thread { get; set; }
        public Post Post { get; set; }
    }
}