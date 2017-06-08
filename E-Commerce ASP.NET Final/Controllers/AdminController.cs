using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce_ASP.NET_Final.Models;

namespace E_Commerce_ASP.NET_Final.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            return View();
        }

        public ActionResult NewCategory(string parentId)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");
            
            ViewBag.parentId = parentId == null ? -1 : Convert.ToInt32(parentId);

            
            if (ViewBag.parentId != -1)
            {
                var db = new CategoryModel();
                ViewBag.parent = db.Category.Find(ViewBag.parentId);
            }
                

            return View();
        }

        public ActionResult EditCategory(int id)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            var db = new CategoryModel();
            ViewBag.Category = db.Category.Find(id);

            return View();
        }

        public ActionResult Order(int id)
        {
            var db = new OrderModel();
            Order order = db.Order.Find(id);
            return View(order);
        }
    }
}