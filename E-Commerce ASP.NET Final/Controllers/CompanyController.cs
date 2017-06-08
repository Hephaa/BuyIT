using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_ASP.NET_Final.Models;

namespace E_Commerce_ASP.NET_Final.Controllers
{
    public class CompanyController : Controller
    {
        private CategoryModel db = new CategoryModel();

        // GET: Company
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Company/Details/5
        public ActionResult Show(int? id, int? page, int? cat, int? order)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }

            //Handle
            var categoryDb = new CategoryModel();

            var productDb = new ProductModel();

            //get products

            List<Product> productList = productDb.Product.Where(x => x.CompanyId == id).ToList();

            //check parameters
            List<Product> unProductList = productList.Where(x => ((cat == null || cat == -1) || x.CategoryId == cat)).ToList();

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

            ViewBag.ProdcutCount = oProductList.Count;

            //Setting the page
            int pageNumber = page ?? 1;

            ViewBag.Page = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double)oProductList.Count / 15);

            int limit = (pageNumber) * 15 >= oProductList.Count ? (oProductList.Count - (pageNumber - 1) * 15) : 15;

            ViewBag.ProductList = oProductList.GetRange((pageNumber - 1) * 15, limit);


            //get the categories from the products

            List<Category> categoryList = new List<Category>();

            List<Int32> idList = new List<int>();


            foreach (Product p in productList)
            {
                if (!idList.Contains(p.CategoryId))
                {
                    idList.Add(p.CategoryId);
                    categoryList.Add(categoryDb.Category.Find(p.CategoryId));
                }
            }

            ViewBag.CategoryList = categoryList;
            return View(company);
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(company);
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}