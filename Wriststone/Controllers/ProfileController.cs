using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wriststone.Models;
using Wriststone.Models.Table;

namespace Wriststone.Controllers
{
    public class ProfileController : Controller
    {
        readonly WriststoneContext db = new WriststoneContext();
        [HttpGet]
        public ActionResult Profile(long? id)
        {
            if(id == null) return View("NotFound");
            return View(db.Accounts.Where(e => e.Id == id).Single());
        }

        public ActionResult SignUp()
        {
            return View(Request.UrlReferrer);
        }

        [HttpPost]
        public ActionResult SignUp(string login, string password, Uri redirect, bool? remember)
        {
            Account user;
            using (MD5 md5Hash = MD5.Create()) password = GetMd5Hash(md5Hash, password); 
            try
            {
                user = db.Accounts.Where(e => e.Login == login && e.Password == password).Single();
            }
            catch (InvalidOperationException)
            {
                ViewData["error"] = "Your login and password is invaild. Please try again!";
                return View(redirect);
            }
            Session["login"] = user.Login;
            Session["id"] = user.Id;
            if (remember == true)
            {
                HttpCookie cookie = new HttpCookie("user");
                cookie.Value = Session["login"].ToString();
                cookie.Expires.AddDays(20);
                Response.Cookies.Add(cookie);
            }
            if (redirect == null) return RedirectToAction("Profile", new { id = user.Id });
            return Redirect(redirect.ToString());
        }

        public ActionResult Logout()
        {
            try
            { 
                string name = Request.Cookies["user"].Name;
                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            catch (NullReferenceException) { }
            Session.Abandon();
            return RedirectToAction("Store", "");
        }

        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(string login, string email, string password, string fullname)
        {
            if (db.Accounts.Where(e => e.Login.ToLower().Equals(login.ToLower())).DefaultIfEmpty().Single() != null)
            {
                ViewData["error"] = "This login is already exists. Try different login!";
                return View();
            }
            else if (db.Accounts.Where(e => e.Email.ToLower().Equals(email.ToLower())).DefaultIfEmpty().Single() != null)
            {
                ViewData["error"] = "This email is already exists. Try different email!";
                return View();
            }
            using (MD5 md5Hash = MD5.Create()) password = GetMd5Hash(md5Hash, password);
            db.Accounts.Add(new Account { Login = login, Email = email, Password = password, Fullname = fullname, Created = DateTime.Now });
            db.SaveChanges();
            var user = db.Accounts.Where(e => e.Login == login && e.Password == password).Single();
            Session["login"] = user.Login;
            Session["id"] = user.Id;
            return RedirectToAction("Profile", new { user.Id });
        }

        public ActionResult Edit()
        {
            if(Session["id"] == null) return View("NotFound");
            var login = Convert.ToInt64(Session["id"]);
            return View(db.Accounts.Where(e => e.Id == login).Single());
        }

        [HttpPut]
        public ActionResult Edit(long id, string email, string password, string fullname)
        {
            var user = db.Accounts.Where(e => e.Id == id).Single();
            user.Email = email;
            using (MD5 md5Hash = MD5.Create()) user.Password = GetMd5Hash(md5Hash, password);
            user.Fullname = fullname;
            db.SaveChanges();
            return RedirectToAction("Profile", new { user.Id });
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        public ActionResult Products()
        {
            var login = Convert.ToInt64(Session["id"]);
            var products = from product in db.Products
                           join orderdetails in db.OrderDetails on product.Id equals orderdetails.Product
                           join order in db.Orders on orderdetails.Order equals order.Id
                           join account in db.Accounts on order.Account equals account.Id
                           where account.Id == login
                           select product; 
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult Recovery(string email)
        {
            try
            {
                Account user = db.Accounts.Where(e => e.Email == email).Single();
            }
            catch (InvalidOperationException)
            {
                ViewData["error"] = "Your email is invaild or absent. Please try again!";
                return View("/Profile/Recovery");
            }
            return View("/Profile/PasswordRecovery");
        }
    }
}
