using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using thimau.Models;

namespace thimau.Controllers
{
    public class SinhviensController : Controller
    {
        private thimauEntities db = new thimauEntities();

        // GET: Sinhviens
        public ActionResult Index()
        {
            var sinhviens = db.Sinhviens.Include(s => s.Lop);
            return View(sinhviens.ToList());
        }

        // GET: Sinhviens/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            return View(sinhvien);
        }

        // GET: Sinhviens/Create
        public ActionResult Create()
        {
            ViewBag.Malop = new SelectList(db.Lops, "Malop", "Tenlop");
            return View();
        }

        // POST: Sinhviens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Masv,Hosv,Tensv,Ngaysinh,Gioitinh,Anhsv,Diachi,Malop")] Sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                db.Sinhviens.Add(sinhvien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Malop = new SelectList(db.Lops, "Malop", "Tenlop", sinhvien.Malop);
            return View(sinhvien);
        }

        // GET: Sinhviens/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            ViewBag.Malop = new SelectList(db.Lops, "Malop", "Tenlop", sinhvien.Malop);
            return View(sinhvien);
        }

        // POST: Sinhviens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Masv,Hosv,Tensv,Ngaysinh,Gioitinh,Anhsv,Diachi,Malop")] Sinhvien sinhvien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Malop = new SelectList(db.Lops, "Malop", "Tenlop", sinhvien.Malop);
            return View(sinhvien);
        }

        // GET: Sinhviens/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }
            return View(sinhvien);
        }

        // POST: Sinhviens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sinhvien sinhvien = db.Sinhviens.Find(id);
            db.Sinhviens.Remove(sinhvien);
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
        public ActionResult Gioithieu()
        {
            return View();
        }

        public ActionResult timkiem_mssv()
        {
            var sinhviens = db.Sinhviens.Include(s => s.Lop);
            return View(sinhviens.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult timkiem_mssv(string filter, string filterName)
        {
            var sinhviens = db.Sinhviens.ToList();

            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(filterName))
            {
                sinhviens = sinhviens.Where(s => s.Masv == filter && (s.Hosv + " " + s.Tensv) == filterName).ToList();
            }
            else
            {
                // Return an empty list if either filter or filterName is empty
                sinhviens = new List<Sinhvien>();
            }

            return View(sinhviens);
        }

    }
}
