using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class QuanLyPHIEUNHAPController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /QuanLyPHIEUNHAP/
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NHACUNGCAPs;
            ViewBag.ListSANPHAM = db.SANPHAMs;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PHIEUNHAP model,IEnumerable<CHITIETPHIEUNHAP> lstModel)
        {
            ViewBag.MaNCC = db.NHACUNGCAPs;
            ViewBag.ListSANPHAM = db.SANPHAMs;
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            //model.DaXoa = false;
            db.PHIEUNHAPs.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy được mã phiếu nhập gán cho lstCHITIETPHIEUNHAP
            SANPHAM sp;
            foreach (var item in lstModel)
            {
                //Cập nhật số lượng tồn
                sp = db.SANPHAMs.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuong;
                //Gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
                item.MaPN = model.MaPN;
            }
            db.CHITIETPHIEUNHAPs.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //Danh sách sản phẩm gần hết hàng với số lượng tồn bé hơn hoặc bằng 5
            var lstSP = db.SANPHAMs.Where(n => n.DaXoa == false&&n.SoLuongTon<=5);
            return View(lstSP);
        
        }
        //Tạo 1 view phục vụ cho việc nhập từng sản phẩm
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC") ;
            //Tương tự như trang chỉnh sửa sản phẩm nhưng ta không cần phải show hết các thuộc tính 
            //Chỉ thuộc tính nào cần thiết mà thôi đó là số lượng tồn hình ảnh... thông tin hiển thị cần thiết
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);

        }
        // Xử lý nhập hàng từng sản phẩm
        [HttpPost]
        public ActionResult NhapHangDon(PHIEUNHAP model, CHITIETPHIEUNHAP ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",model.MaNCC);
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            model.ThoiDiemNhap = DateTime.Now;
            //model.DaXoa = false;
            db.PHIEUNHAPs.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy được mã phiếu nhập gán cho lstCHITIETPHIEUNHAP
            ctpn.MaPN = model.MaPN;
            //Cập nhật tồn 
            SANPHAM sp = db.SANPHAMs.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuong;
            db.CHITIETPHIEUNHAPs.Add(ctpn);
            db.SaveChanges();
            return View(sp);

        }
        //Giải phóng biến cho vùng nhớ
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