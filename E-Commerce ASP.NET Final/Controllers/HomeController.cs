using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_ASP.NET_Final.Models;
using Microsoft.Ajax.Utilities;

namespace E_Commerce_ASP.NET_Final.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            var db = new CategoryModel();
            ViewBag.CategoryList = db.Category.Where(e => e.ParentId == -1).ToList();

            var productDb = new ProductModel();

            List<Product> productList = productDb.Product.ToList();

            productList.Reverse();

            //Setting the page
            int pageNumber = page ?? 1;

            ViewBag.Page = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double)productList.Count / 15);

            int limit = (pageNumber) * 15 >= productList.Count ? (productList.Count - (pageNumber - 1) * 15) : 15;

            ViewBag.ProductList = productList.GetRange((pageNumber - 1) * 15, limit);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Order()
        {
            return View();
        }

        public ActionResult Search(string q, int? page, int? cat, int? com, int? order)
        {
            if (q == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SearchTerm = q;

            //Handle
            var categoryDb = new CategoryModel();

            var productDb = new ProductModel();

            //get products

            List<Product> productList = productDb.Product.Where(x => x.Name.ToLower().Contains(q.ToLower()) || x.Description.ToLower().Contains(q.ToLower())).ToList();

            //check parameters
            List<Product> unProductList = productList.Where(x => ((cat == null || cat == -1) || x.CategoryId == cat) && ((com == null || com == -1) || x.CompanyId == com)).ToList();

            //ordering
            int orderType = order ?? 0;

            List<Product> oProductList = new List<Product>();

            switch (orderType) //0 -name 1- ascending 2-descending
            {
                case 0:
                    oProductList = unProductList.OrderBy(o => o.Name).ToList();
                    break;
                case 1:
                    oProductList = unProductList.OrderBy(o => o.Price).ToList();
                    break;
                case 2:
                    oProductList = unProductList.OrderByDescending(o => o.Price).ToList();
                    break;
            }

            ViewBag.OrderType = orderType;

            ViewBag.CategorySelect = cat ?? -1;

            ViewBag.CompanySelect = com ?? -1;

            ViewBag.ProdcutCount = oProductList.Count;

            //Setting the page
            int pageNumber = page ?? 1;

            ViewBag.Page = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double)oProductList.Count / 15);

            int limit = (pageNumber) * 15 >= oProductList.Count ? (oProductList.Count - (pageNumber - 1) * 15) : 15;

            ViewBag.ProductList = oProductList.GetRange((pageNumber - 1) * 15, limit);


            //get the categories and companies from the products

            var companyDb = new CompanyModel();

            List<Category> categoryList = new List<Category>();
            List<Company> companyList = new List<Company>();

            List<Int32> idList = new List<int>();
            List<Int32> cidList = new List<int>();


            foreach (Product p in productList)
            {
                if (!idList.Contains(p.CategoryId))
                {
                    idList.Add(p.CategoryId);
                    categoryList.Add(categoryDb.Category.Find(p.CategoryId));
                }
                if (!cidList.Contains(p.CompanyId))
                {
                    cidList.Add(p.CompanyId);
                    companyList.Add(companyDb.Company.Find(p.CompanyId));
                }
            }

            ViewBag.CategoryList = categoryList;

            ViewBag.CompanyList = companyList;

            return View();
        }

        [HttpPost]
        public ActionResult CompleteOrder()
        {
            string cart = (string)Session["Cart"];
            if (!cart.IsNullOrWhiteSpace())
            {

                //Save order
                var orderDb = new OrderModel();

                Order order = new Order();
                order.Products = cart;

                string email = Request.Form["email"];
                string number = Request.Form["number"];
                string adress = Request.Form["adress"];

                order.Email = email;
                order.PhoneNumber = number;
                order.Adress = adress;
                order.Date = DateTime.Now;

                orderDb.Order.Add(order);

                orderDb.SaveChanges();

                //Update product amounts
                var productDb = new ProductModel();

                int[][] arr = Helpers.StringToCartArray(cart);

                for (int i = 0; i < arr.Length; i++)
                {
                    Product p = productDb.Product.Find(arr[i][0]);
                    p.Amount -= arr[i][1];
                }

                productDb.SaveChanges();

                //Clean cart
                Session["Cart"] = "";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Cart()
        {
            ViewBag.Cart = ((string)Session["Cart"]).IsNullOrWhiteSpace() ? null : Helpers.StringToCartArray((string)Session["Cart"]);

            return View();
        }

        public ActionResult AddToCart(int id, int amount, int type) //Type -> 0: add 1:change
        {
            string cart = (string)Session["Cart"];// productid1-amount/productid2-amount2  :  15-3/22-1
            if (!cart.IsNullOrWhiteSpace())
            {
                int[][] cartArray = Helpers.StringToCartArray(cart);
                

                for (int i = 0; i < cartArray.Length; i++)
                {
                    if (cartArray[i][0] == id)
                    {
                        if (type == 1)
                            cartArray[i][1] = amount;
                        else cartArray[i][1] += amount;

                        var productDb = new ProductModel();
                        Product p = productDb.Product.Find(cartArray[i][0]);
                        if (p.Amount < cartArray[i][1]) cartArray[i][1] = p.Amount;

                        Session.Add("Cart", Helpers.CartArrayToString(cartArray));
                        return RedirectToAction("Cart");
                    }
                }

                cart += "/" + id + "-" + amount;
            }
            else
            {
                cart = id + "-" + amount;
            }
            Session.Add("Cart", cart);

            return RedirectToAction("Cart");
        }

        public ActionResult RemoveFromCart(int id)
        {
            string cart = (string)Session["Cart"];// productid1-amount/productid2-amount2  :  15-3/22-1
            if (!cart.IsNullOrWhiteSpace())
            {
                int[][] cartArray = Helpers.StringToCartArray(cart);


                for (int i = 0; i < cartArray.Length; i++)
                {
                    if (cartArray[i][0] == id)
                    {
                        cartArray = cartArray.Where(x => x != cartArray[i]).ToArray();
                        Session.Add("Cart", Helpers.CartArrayToString(cartArray));

                        if(cartArray.Length == 0)
                            return RedirectToAction("Index");
                        else
                            return RedirectToAction("Cart");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            Session.Add("Cart", cart);

            return RedirectToAction("Cart");
        }



    }
}