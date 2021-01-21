using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wriststone.Models;
using Wriststone.Models.Table;
using Wriststone.Models.ViewModel;

namespace Wriststone.Controllers
{
    public class StoreController : Controller
    {
        WriststoneContext db = new WriststoneContext();
        // GET: Store
        public ActionResult Store()
        {
            return View(db.ProductCategories.Where(e => e.Category == null).ToList());
        }

        [HttpGet]
        public ActionResult Category(long? id)
        {
            if (id == null) return View("NotFound");
            CategoryProductsModel categoryProducts = new CategoryProductsModel
            {
                Categories = db.ProductCategories.Where(e => e.Category == id).ToList().AsEnumerable(),
                Products = db.Products.Where(e => e.Category == id).ToList().AsEnumerable()
            };
            return View(categoryProducts);
        }

        [HttpGet]
        public ActionResult Product(long? id)
        {
            if (id == null) return View("NotFound");
            ProductModel productModel = new ProductModel();
            productModel.Product = db.Products.Where(e => e.Id == id).Single();
            try
            {
                productModel.Rate = db.Ratings.Where(e => e.Product == productModel.Product.Id).Average(e => e.Rate).Value;
            }
            catch(InvalidOperationException)
            {
                productModel.Rate = 0.0;
            }
            if (Session["id"] != null)
            {
                var login = Convert.ToInt64(Session["id"]);
                var orders = from account in db.Accounts
                             join order in db.Orders on account.Id equals order.Account
                             join orderdetails in db.OrderDetails on order.Id equals orderdetails.Order
                             join product in db.Products on orderdetails.Product equals product.Id
                             where account.Id == login && product.Id == id
                             select orderdetails;
                if (orders.ToList().Count() > 0) productModel.Bought = true;
            }
            var result = from product in db.Products
                         join rating in db.Ratings on product.Id equals rating.Product
                         join account in db.Accounts on rating.Account equals account.Id
                         where product.Id == id
                         select new AccountRatingModel { Rating = rating, Account = account };
            productModel.Ratings = result.ToList().AsEnumerable();
            return View(productModel);
        }

        [HttpPost]
        public ActionResult Comment(string comment, long product)
        {
            var login = Session["login"].ToString();
            var account = db.Accounts.Where(e => e.Login.Equals(login)).Single().Id;
            var rating = db.Ratings.Where(e => e.Account == (account)).DefaultIfEmpty().Single();
            if(rating == null) db.Ratings.Add(new Rating { Message = comment, Account = account, Product = product });
            else if (rating.Message != null || rating.Rate != null) rating.Message = comment;
            else db.Ratings.Add(new Rating { Message = comment, Account = account, Product = product });
            db.SaveChanges();
            return RedirectToAction("Product", new { Id = product });
        }

        [HttpPost]
        public ActionResult Rate(int rank, long product)
        {
            var login = Session["login"].ToString();
            var account = db.Accounts.Where(e => e.Login.Equals(login)).Single().Id;
            var rating = db.Ratings.Where(e => e.Account == (account)).DefaultIfEmpty().Single();
            if (rating == null) db.Ratings.Add(new Rating { Rate = rank, Account = account, Product = product });
            else if (rating.Message != null || rating.Rate != null) rating.Rate = rank;
            else db.Ratings.Add(new Rating { Rate = rank, Account = account, Product = product });
            db.SaveChanges();
            return RedirectToAction("Product", new { Id = product });
        }

        public ActionResult Cart(long? id)
        {
            if (id == null) return View("NotFound");
            if (Session["cart"] == null)
            {
                List<Product> cart = new List<Product> { db.Products.Where(e => e.Id == id).Single() };
                Session["cart"] = cart;
            }
            else
            {
                List<Product> cart = (List<Product>)Session["cart"];
                if(!CheckExist(id)) cart.Add(db.Products.Where(e => e.Id == id).Single());
                Session["cart"] = cart;
            }
            return RedirectToAction("Product", new { Id = id});
        }

        private bool CheckExist(long? id)
        {
            List<Product> cart = (List<Product>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Id == id) return true;
            return false;
        }
    }
}
