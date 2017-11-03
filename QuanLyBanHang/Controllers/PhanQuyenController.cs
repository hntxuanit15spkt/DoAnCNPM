using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class PhanQuyenController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            
            return View(db.LOAINGUOIDUNGs.OrderBy(n=>n.TenLoaiNguoiDung));
        }
        [HttpGet]
        //public ActionResult PhanQuyen(int? id)
        //{
        //    //Lấy mã loại tv dựa vào id
        //    if (id == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    LOAINGUOIDUNG ltv = db.LOAINGUOIDUNGs.SingleOrDefault(n => n.MaLoaiTV == id);
        //    if (ltv == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    //Lấy ra danh sách quyền để load ra check box
        //    ViewBag.MaQuyen = db.Quyens;
        //    //Lấy ra danh sách quyền của loại thành viên đó
        //    //Bước 1: Lấy ra những quyền thuộc loại thành viên đó dựa vào bảng LOAINGUOIDUNG_Quyen
        //    ViewBag.LoaiTVQuyen = db.LOAINGUOIDUNG_Quyen.Where(n => n.MaLoaiTV == id);
        //    return View(ltv);
        //}
        [HttpPost]
        //public ActionResult PhanQuyen(int? MaLTV, IEnumerable<LOAINGUOIDUNG_Quyen> lstPhanQuyen)
        //{

        //     //Trường hợp : Nếu đã đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
        //     //Bước 1: Xóa những quyền cũa thuộc loại TV đó
        //    var lstDaPhanQuyen = db.LOAINGUOIDUNG_Quyen.Where(n => n.MaLoaiTV == MaLTV);
        //    if (lstDaPhanQuyen.Count() !=0)
        //    {
        //        db.LOAINGUOIDUNG_Quyen.RemoveRange(lstDaPhanQuyen);
        //        db.SaveChanges();
        //    }
        //    if (lstPhanQuyen != null)
        //    {
        //        //Kiểm tra list danh sách quyền được check
        //        foreach (var item in lstPhanQuyen)
        //        {
        //            item.MaLoaiTV = int.Parse(MaLTV.ToString());
        //            //Nếu được check thì insert dữ liệu vào bảng phân quyền
        //            db.LOAINGUOIDUNG_Quyen.Add(item);


        //        }
        //        db.SaveChanges();
        //    }
           
        //    return RedirectToAction("Index");
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}