using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
	public class GioHangController : Controller
	{

		QuanLyBanHangEntities db = new QuanLyBanHangEntities();
		//Lấy giỏ hàng
		public List<ItemGioHang> LayGioHang()
		{
			//Giỏ hàng đã tồn tại 
			List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
			if (lstGioHang == null)
			{
				//Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
				lstGioHang = new List<ItemGioHang>();
				Session["GioHang"] = lstGioHang;
			}
			return lstGioHang;
		}
		//Thêm giỏ hàng thông thường (Load lại trang)
		public ActionResult ThemGioHang(int MaSP, string strURL)
		{
			//Kiểm tra sản phẩm có tồn tại trong CSDL hay không
			SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
			if (sp == null)
			{
				//TRang đường dẫn không hợp lệ
				Response.StatusCode = 404;
				return null;
			}
			//Lấy giỏ hàng
			List<ItemGioHang> lstGioHang = LayGioHang();
			//Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng 
			ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
			if (spCheck != null)
			{
				//Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
				if (sp.SoLuongTon < spCheck.SoLuong)
				{
					return View("ThongBao");
				}
				spCheck.SoLuong++;
				spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
				return Redirect(strURL);
			}

			ItemGioHang itemGH = new ItemGioHang(MaSP);
			if (sp.SoLuongTon < itemGH.SoLuong)
			{
				return View("ThongBao");
			}

			lstGioHang.Add(itemGH);
			return Redirect(strURL);
		}
		//Tính tổng số lượng
		public double TinhTongSoLuong()
		{
			//Lấy giỏ hàng
			List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
			if (lstGioHang == null)
			{
				return 0;
			}
			return lstGioHang.Sum(n => n.SoLuong);
		}
		//Tính Tổng tiền 
		public decimal TinhTongTien()
		{
			//Lấy giỏ hàng
			List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
			if (lstGioHang == null)
			{
				return 0;
			}
			return lstGioHang.Sum(n => n.ThanhTien);
		}

		public ActionResult GioHangPartial()
		{
			if (TinhTongSoLuong() == 0)
			{
				ViewBag.TongSoLuong = 0;
				ViewBag.TongTien = 0;
				return PartialView();
			}
			ViewBag.TongSoLuong = TinhTongSoLuong();
			ViewBag.TongTien = TinhTongTien();
			return PartialView();
		}

		// GET: /GioHang/
		public ActionResult XemGioHang()
		{
			//Lấy giỏ hàng 
			List<ItemGioHang> lstGioHang = LayGioHang();
			return View(lstGioHang);
		}

		//Chỉnh sửa giỏ hàng
		[HttpGet]
		public ActionResult SuaGioHang(int MaSP)
		{
			//Kiểm tra session giỏ hàng tồn tại chưa 
			if (Session["GioHang"] == null)
			{
				return RedirectToAction("Index", "Home");
			}
			//Kiểm tra sản phẩm có tồn tại trong CSDL hay không
			SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
			if (sp == null)
			{
				//TRang đường dẫn không hợp lệ
				Response.StatusCode = 404;
				return null;
			}
			//Lấy list giỏ hàng từ session
			List<ItemGioHang> lstGioHang = LayGioHang();
			//Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
			ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
			if (spCheck == null)
			{
				return RedirectToAction("Index", "Home");
			}
			//Lấy list giỏ hàng tạo giao diện
			ViewBag.GioHang = lstGioHang;

			//Nếu tồn tại rồi
			return View(spCheck);
		}
		//Xử lý cập nhật giỏ hàng
		[HttpPost]
		public ActionResult CapNhatGioHang(ItemGioHang itemGH)
		{
			//Kiểm tra số lượng tồn 
			SANPHAM spCheck = db.SANPHAMs.Single(n => n.MaSP == itemGH.MaSP);
			if (spCheck.SoLuongTon < itemGH.SoLuong)
			{
				return View("ThongBao");
			}
			//Cập nhật số lượng trong session giỏ hàng 
			//Bước 1: Lấy List<GioHang> từ session["GioHang"]
			List<ItemGioHang> lstGH = LayGioHang();
			//Bước 2: Lấy sản phẩm cần cập nhật từ trong list<GioHang> ra
			ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
			//Bước 3: Tiến hành cập nhật lại số lượng cũng thành tiền
			itemGHUpdate.SoLuong = itemGH.SoLuong;
			itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
			return RedirectToAction("XemGioHang");
		}

		public ActionResult XoaGioHang(int MaSP)
		{
			//Kiểm tra session giỏ hàng tồn tại chưa 
			if (Session["GioHang"] == null)
			{
				return RedirectToAction("Index", "Home");
			}
			//Kiểm tra sản phẩm có tồn tại trong CSDL hay không
			SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
			if (sp == null)
			{
				//TRang đường dẫn không hợp lệ
				Response.StatusCode = 404;
				return null;
			}
			//Lấy list giỏ hàng từ session
			List<ItemGioHang> lstGioHang = LayGioHang();
			//Kiểm tra xem sản phẩm đó có tồn tại trong giỏ hàng hay không
			ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
			if (spCheck == null)
			{
				return RedirectToAction("Index", "Home");
			}
			//Xóa item trong giỏ hàng
			lstGioHang.Remove(spCheck);
			return RedirectToAction("XemGioHang");
		}
		//Xây dựng chức năng đặt hàng
		//public ActionResult DatHang(KHACHHANG kh)
		//{
		//	//Kiểm tra session giỏ hàng tồn tại chưa 
		//	if (Session["GioHang"] == null)
		//	{
		//		return RedirectToAction("Index", "Home");
		//	}
		//	KHACHHANG khang = new KHACHHANG();
		//	if (Session["TaiKhoan"] == null)
		//	{
		//		//Thêm khách hàng vào bảng khách hàng đối với khách hàng vãng lai (kh chưa có tài khoản)
		//		khang = kh;
		//		db.KHACHHANGs.Add(khang);
		//		db.SaveChanges();
		//	}
		//	else
		//	{
		//		//Đối với khách hàng là thành viên
		//		NGUOIDUNG tv = Session["TaiKhoan"] as NGUOIDUNG;
		//		khang.TenKH = tv.HoTen;
		//		khang.DiaChi = tv.DiaChi;
		//		khang.Email = tv.Email;
		//		khang.SoDienThoai = tv.SoDienThoai;
		//		khang.MaNGUOIDUNG = tv.MaLoaiTV;
		//		db.KHACHHANGs.Add(khang);
		//		db.SaveChanges();
		//	}

		//	//Thêm đơn hàng 
		//	DONDATHANG ddh = new DONDATHANG();
		//	ddh.MaKH = khang.MaKH;
		//	ddh.NgayDat = DateTime.Now;
		//	ddh.TinhTrangGiaoHang = false;
		//	ddh.DaThanhToan = false;
		//	ddh.UuDai = 0;
		//	ddh.DaHuy = false;
		//	ddh.DaXoa = false;
		//	db.DONDATHANGs.Add(ddh);
		//	db.SaveChanges();
		//	//Thêm chi tiết đơn đặt hàng
		//	List<ItemGioHang> lstGH = LayGioHang();
		//	foreach (var item in lstGH)
		//	{
		//		CHITIETDONDATHANG ctdh = new CHITIETDONDATHANG();
		//		ctdh.MaDDH = ddh.MaDDH;
		//		ctdh.MaSP = item.MaSP;
		//		ctdh.TenSP = item.TenSP;
		//		ctdh.SoLuong = item.SoLuong;
		//		ctdh.DonGia = item.DonGia;
		//		db.CHITIETDONDATHANGs.Add(ctdh);
		//	}
		//	db.SaveChanges();
		//	Session["GioHang"] = null;
		//	return RedirectToAction("XemGioHang");
		//}
		//Thêm giỏ hàng Ajax
		public ActionResult ThemGioHangAjax(int MaSP, string strURL)
		{
			//Kiểm tra sản phẩm có tồn tại trong CSDL hay không
			SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP);
			if (sp == null)
			{
				//TRang đường dẫn không hợp lệ
				Response.StatusCode = 404;
				return null;
			}
			//Lấy giỏ hàng
			List<ItemGioHang> lstGioHang = LayGioHang();
			//Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng 
			ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
			if (spCheck != null)
			{
				//Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
				if (sp.SoLuongTon < spCheck.SoLuong)
				{
					ViewBag.ThongBao = 0;
					ViewBag.TongSoLuong = TinhTongSoLuong();
					ViewBag.TongTien = TinhTongTien();
					//return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
					return PartialView("GioHangPartial");
				}
				spCheck.SoLuong++;
				spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
				ViewBag.TongSoLuong = TinhTongSoLuong();
				ViewBag.TongTien = TinhTongTien();
				return PartialView("GioHangPartial");
			}
			ItemGioHang itemGH = new ItemGioHang(MaSP);
			if (sp.SoLuongTon < itemGH.SoLuong)
			{
				//return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
				ViewBag.TongSoLuong = TinhTongSoLuong();
				ViewBag.TongTien = TinhTongTien();
				ViewBag.ThongBao = 0;
				return PartialView("GioHangPartial");
			}
			lstGioHang.Add(itemGH);
			ViewBag.TongSoLuong = TinhTongSoLuong();
			ViewBag.TongTien = TinhTongTien();
			return PartialView("GioHangPartial");
		}
	}
}