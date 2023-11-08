using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using thuchanh.Library;

namespace thuchanh.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Image,Slug,Order,Fullname,Phone,Email,MetaDesc,MetaKey,CreateBy,CreateAt,UpdateBy,UpdateAt,Status")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động 1 số trường
                //-----Create At
                suppliers.CreateAt = DateTime.Now;
                //-----Create By
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //-----Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //-----UpdateAt
                suppliers.UpdateAt = DateTime.Now;
                //-----UpdateBy
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //Xử lý thông tin phần Hình ảnh
                var img = Request.Files["img"];//Lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//Lấy phần mở rộng tập tin
                    {
                        string slug = suppliers.Slug;
                        //Tên file = Slug + Phần mở rộng của tập tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //Upload hình
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//Kết thúc upload hình ảnh
                //Thêm mới nhà cung cấp vào DB
                suppliersDAO.Insert(suppliers);
                //Hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: CreateAt/By, UpdateAt/By, Slug, OrderBy

                //Xu ly tu dong cho: UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Xu ly tu dong cho: Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }

                //Xu ly tu dong cho: Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //truoc khi cap nhat lai anh moi thi xoa anh cu
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier/";
                if (img.ContentLength != 0 && suppliers.Image != null)//ton tai mot logo cua NCC tu truoc
                {
                    //xoa anh cu
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
                //upload anh moi cua NCC
                //xu ly cho phan upload hinh anh

                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;//abc-def-1.jpg
                        //upload hinh
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //cap nhat mau tin vao DB
                suppliersDAO.Update(suppliers);
                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật mẩu tin thành công");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            suppliersDAO.Delete(suppliers);
            TempData["message"] = new XMessage("success", "Xoá nhà cung cấp thành công");
            return RedirectToAction("Index");
        }
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xoá nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái
            suppliers.Status = 0;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Xoá nhà cung cấp thành công");
            //Trở về trang Index
            return RedirectToAction("Index");
        }
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash : Thùng rác
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Phục hồi nhà cung cấp thất bại");
                //Chuyển hướng trang
                return RedirectToAction("Index");
            }
            //Cập nhật trạng thái status = 2
            suppliers.Status = 2;
            //Cập nhật UpdateBy
            suppliers.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
            //Cập nhật UpdateAt
            suppliers.UpdateAt = DateTime.Now;
            //Update Database
            suppliersDAO.Update(suppliers);
            //Hiển thị thông báo thành công
            TempData["message"] = new XMessage("success", "Phục hồi nhà cung cấp thành công");
            //Ở lại trang Trash để xoá tiếp
            return RedirectToAction("Trash");
        }

    }
}
