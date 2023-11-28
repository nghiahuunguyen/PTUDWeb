using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using ThuchanhPTUDW.Library;

namespace ThuchanhPTUDW.Areas.Admin.Controllers
{
    public class TopicController : Controller
    {
        TopicsDAO TopicsDAO =new TopicsDAO();
        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(TopicsDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");

            }
            return View(Topics);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(TopicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(TopicsDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topics Topics)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                Topics.CreateAt = DateTime.Now;
                //-----Create By
                Topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                Topics.Slug = XString.Str_Slug(Topics.Name);
                //-----ParentID
                if (Topics.ParentId == null)
                {
                    Topics.ParentId = 0;
                }
                //-----Order
                if (Topics.Order == null)
                {
                    Topics.Order = 1;
                }
                else
                {
                    Topics.Order += 1;
                }
                //-----UpdateAt
                Topics.UpdateAt = DateTime.Now;
                //-----UpdateBy
                Topics.UpdateBy = Convert.ToInt32(Session["UserID"]);
                TopicsDAO.Insert(Topics);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Tạo mới sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(TopicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(TopicsDAO.getList("Index"), "Order", "Name");
            return View(Topics);
        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(TopicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(TopicsDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            return View(Topics);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topics Topics)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //-----Create At
                Topics.CreateAt = DateTime.Now;
                //-----Create By
                Topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //-----Slug
                Topics.Slug = XString.Str_Slug(Topics.Name);
                //-----ParentID
                if (Topics.ParentId == null)
                {
                    Topics.ParentId = 0;
                }
                //-----Order
                if (Topics.Order == null)
                {
                    Topics.Order = 1;
                }
                else
                {
                    Topics.Order += 1;
                }
                //-----UpdateAt
                Topics.UpdateAt = DateTime.Now;
                //-----UpdateBy
                Topics.UpdateBy = Convert.ToInt32(Session["UserID"]);
                TopicsDAO.Update(Topics);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(TopicsDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(TopicsDAO.getList("Index"), "Order", "Name");
            return View(Topics);


        }

        //////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Trash");
            }
            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Trash");
            }
            return View(Topics);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topics Topics = TopicsDAO.getRow(id);
            //tim thay mau tin, xoa
            TopicsDAO.Delete(Topics);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");

            return RedirectToAction("Trash");
        }

        // GET: Admin/Category/Status/5

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái
            Topics.Status = (Topics.Status == 1) ? 2 : 1;
            ///cập nhật updateAt  
            Topics.UpdateAt = DateTime.Now;
            ///cập nhật update By
            Topics.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            TopicsDAO.Update(Topics);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        // GET: Admin/Category/DelTrash/5

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái
            Topics.Status = 0;
            ///cập nhật updateAt  
            Topics.UpdateAt = DateTime.Now;
            ///cập nhật update By
            Topics.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            TopicsDAO.Update(Topics);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            //tro ve trang index
            return RedirectToAction("Index");

        }
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(TopicsDAO.getList("Trash"));
        }
        // GET: Admin/Category/Undo/5

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }

            Topics Topics = TopicsDAO.getRow(id);
            if (Topics == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            ///Cập nhật trạng thái status = 2
            Topics.Status = 2;
            ///cập nhật updateAt  
            Topics.UpdateAt = DateTime.Now;
            ///cập nhật update By
            Topics.CreateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            TopicsDAO.Update(Topics);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
            //tro ve trang index
            return RedirectToAction("Trash");

        }
    }
}