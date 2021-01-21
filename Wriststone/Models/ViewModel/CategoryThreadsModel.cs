using System.Collections.Generic;
using System.Linq;
using Wriststone.Models.Table;

namespace Wriststone.Models.ViewModel
{
    public class CategoryThreadsModel
    {
        public ForumCategory Category { get; set; }
        public IEnumerable<Thread> Threads { get; set; }
        public IEnumerable<ForumCategory> Categories { get; set; }
    }
}