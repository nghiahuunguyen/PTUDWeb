using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;

namespace thuchanh.Controllers
{
    public class SiteController : Controller
    {
        // GET: Sie
        public ActionResult Index()
        {
            MyDBContext db = new MyDBContext();//tao moi DB
            int sodong = db.Products.Count();
            ViewBag.sodong = sodong;
            return View();
        }
    }
}