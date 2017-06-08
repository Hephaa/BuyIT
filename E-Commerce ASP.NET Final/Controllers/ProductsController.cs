using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_ASP.NET_Final.Models;

namespace E_Commerce_ASP.NET_Final.Controllers
{
    public class ProductsController : Controller
    {
        private CategoryModel db = new CategoryModel();
        
        // GET: Products//5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create(int category)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            ViewBag.category = category;

            var categoryDb = new CategoryModel();
            Category cat = categoryDb.Category.Find(category);
            ViewBag.CategoryName = cat.Name;
        
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,CompanyId,Name,Description,Price,Amount")] Product product)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //Add product
                db.Products.Add(product);
                db.SaveChanges();

                //Add the image
                HttpPostedFileBase file = Request.Files["pic"];
                if (file.ContentLength > 0)
                {
                    Image image = Image.FromStream(file.InputStream);
                    Bitmap bmp = new Bitmap(image);

                    ImageHandler.SaveImage(bmp, product.Id);
                }

                return RedirectToAction("Show", "Products", new {id = product.Id});
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,CompanyId,Name,Description,Price,Amount")] Product product)
        {
            if(!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                //Change the image
                HttpPostedFileBase file = Request.Files["pic"];
                if (file.ContentLength > 0)
                {
                    Image image = Image.FromStream(file.InputStream);
                    Bitmap bmp = new Bitmap(image);

                    ImageHandler.SaveImage(bmp, product.Id);
                }

                return RedirectToAction("Show", "Products", new { id = product.Id });
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!Request.IsAuthenticated) return RedirectToAction("Login", "Account");

            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
