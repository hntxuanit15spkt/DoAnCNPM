using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;
namespace WebSiteBanHang.Controllers
{
    public class SANPHAMController : Controller
    {
        //
        // GET: /SANPHAM/
        //Sử dụng partial view
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo 2 style khác nhau
        [ChildActionOnly]
        public ActionResult SANPHAMStyle1Partial()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SANPHAMStyle2Partial()
        {

            return PartialView();
        }
        //Xây dựng trang xem chi tiết 
        public ActionResult XemChiTiet(int? id,string tensp)
        {
            //Kiểm tra tham số truyền vào có rổng hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }
        //Xây dựng 1 action load sản phẩm theo mã loại sản phẩm và mã nhà sản xuất
        //public ActionResult SANPHAM(int? MaLoaiSP, int? MaNSX,int? page)
        //{
        //    //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
        //    //{
        //    //    return RedirectToAction("Index","Home");
        //    //}

        //    //if (MaLoaiSP == null || MaNSX==null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    /*Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường
        //    trong bảng sản phẩm */
        //    //var lstSP = db.SANPHAMs.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
        //    //if (lstSP.Count() == 0)
        //    //{
        //    //    //Thông báo nếu như không có sản phẩm đó
        //    //    return HttpNotFound();  
        //    //}
        //    //Thực hiện chức năng phân trang
        //    //Tạo biến số sản phẩm trên trang
        //    //int PageSize = 2;
        //    ////Tạo biến thứ 2: Số trang hiện tại
        //    //int PageNumber = (page ?? 1);
        //    //ViewBag.MaLoaiSP = MaLoaiSP;
        //    //ViewBag.MaNSX = MaNSX;
        //    //return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,PageSize));
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