using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wriststone.Models;
using Wriststone.Models.Context;

namespace Wriststone.Controllers
{
    public class CartController : Controller
    {
        private WriststoneContext db = new WriststoneContext();
        private Payment payment;
        // GET: Cart
        public ActionResult Cart()
        {
           return View();
        }

        [HttpPost]
        public ActionResult Delete(long? id)
        {
            List<Models.Table.Product> cart = (List<Models.Table.Product>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Id == id) cart.RemoveAt(i);
            if (cart.Count == 0) Session["cart"] = null;
            else Session["cart"] = cart;
            return RedirectToAction("Cart");
        }

        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };
            List<Models.Table.Product> cart = (List<Models.Table.Product>)Session["cart"];
            foreach (var item in cart) listItems.items.Add(new Item
            {
                name = item.Name, currency = "USD", price = item.Price.ToString(), quantity = "1", sku = "sku"
            });
            var payer = new Payer { payment_method = "paypal" };
            var details = new Details
            {
                tax = "0", shipping = "0", subtotal = cart.Sum(e => e.Price).ToString()
            };
            var amount = new Amount
            {
                currency = "USD",
                total = Convert.ToDecimal(details.subtotal).ToString(),
                details = details
            };

            var redirectUrls = new RedirectUrls
            {
                return_url = redirectUrl,
                cancel_url = redirectUrl
            };


            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction
            {
                description = "Wriststone test",
                invoice_number = Convert.ToString(new Random().Next(100000)),
                amount = amount,
                item_list = listItems
            });
            var payment = new Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            return payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution { payer_id = payerId };
            payment = new Payment { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPaypal()
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if(string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPaypal?";
                    var guid = Convert.ToString(new Random().Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while(links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url")) paypalRedirectUrl = link.href;
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (!executedPayment.state.ToLower().Equals("approved"))
                    {
                        Session["cart"] = null;
                        return View("Failure");
                    }

                }
            }
            catch(Exception)
            {
                Session["cart"] = null;
                return View("Failure");
            }
            AcceptOrder();
            Session["cart"] = null;
            return View("Success");
        }

        private void AcceptOrder()
        {
            var account = Convert.ToInt64(Session["id"]);
            var date = DateTime.Now;
            var order = new Models.Table.Order
            {
                Account = account,
                Payment = "Paypal",
                Price = ((List<Models.Table.Product>)Session["cart"]).Sum(e => e.Price),
                Date = date
            };
            db.Orders.Add(order);
            db.SaveChanges();
            order = db.Orders.Where(e => e.Account == account && e.Date == date).Single();
            var cart = (List<Models.Table.Product>)Session["cart"];
            foreach (var item in cart)
            {
                db.OrderDetails.Add(new Models.Table.OrderDetails
                {
                    Order = order.Id,
                    Price = item.Price,
                    Product = item.Id
                });
            }
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult FreeProduct(long id)
        {
            var account = Convert.ToInt64(Session["id"]);
            var date = DateTime.Now;
            var product = db.Products.Where(e => e.Id == id).Single();
            var order = new Models.Table.Order
            {
                Account = account,
                Payment = "Free",
                Price = 0,
                Date = date
            };
            db.Orders.Add(order);
            db.SaveChanges();
            order = db.Orders.Where(e => e.Account == account && e.Date == date).Single();
            db.OrderDetails.Add(new Models.Table.OrderDetails
            {
                Order = order.Id,
                Price = product.Price,
                Product = product.Id
            });
            db.SaveChanges();
            return View("Success");
        }
    }
}