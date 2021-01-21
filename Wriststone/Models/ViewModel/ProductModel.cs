using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wriststone.Models.Table;

namespace Wriststone.Models.ViewModel
{
    public class ProductModel
    {
        public Product Product { get; set; }
        public double Rate { get; set; }
        public IEnumerable<AccountRatingModel> Ratings { get; set; }
        public bool Bought { get; set; }
    }
}