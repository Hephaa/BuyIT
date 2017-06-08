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
    public class CategoryController : Controller
    {
        private CategoryModel db = new CategoryModel();

        // GET: Category
        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Show(int? id, int? page,int? com, int? order)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryDb = new CategoryModel();

            ViewBag.CategoryList = categoryDb.Category.Where(x => x.ParentId == id).ToList();

            var productDb = new ProductModel();

            List<Product> productList = productDb.Product.Where(x => x.CategoryId == id).ToList();

            if (productList.Count == 0) //Get products from child categories
            {
                foreach (Category c in ViewBag.CategoryList)
                {
                    productList.AddRange(productDb.Product.Where(x => x.CategoryId == c.Id));
                }
            }

            //check parameters
            List<Product> unProductList = productList.Where(x => ((com == null || com == -1) || x.CompanyId == com)).ToList();

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

            ViewBag.CompanySelect = com ?? -1;

            ViewBag.ProdcutCount = oProductList.Count;

            //Setting the page
            int pageNumber = page ?? 1;

            ViewBag.Page = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double) oProductList.Count/15);

            int limit = (pageNumber)*15 >= oProductList.Count ? (oProductList.Count - (pageNumber - 1)*15) : 15;

            ViewBag.ProductList = oProductList.GetRange((pageNumber - 1)*15, limit);


            //get the companies from the products

            var companyDb = new CompanyModel();

            List<Company> companyList = new List<Company>();

            List<Int32> idList = new List<int>();


            foreach (Product p in productList)
            {
                if (!idList.Contains(p.CompanyId))
                {
                    idList.Add(p.CompanyId);
                    companyList.Add(companyDb.Company.Find(p.CompanyId));
                }
            }

            ViewBag.CompanyList = companyList;


            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create(string parentId)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            ViewBag.parentId = parentId == null ? -1 : Convert.ToInt32(parentId);


            if (ViewBag.parentId != -1)
            {
                ViewBag.parent = db.Category.Find(ViewBag.parentId);
            }

            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentId,Name")] Category category)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentId,Name")] Category category)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Request.IsAuthenticated) return Redirect("/Account/Login");

            Category category = db.Category.Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
