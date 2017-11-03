using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{

	public class HomeController : Controller
	{
		//
		// GET: /Home/
		QuanLyBanHangEntities db = new QuanLyBanHangEntities();
		public ActionResult Index()
		{
			//Lần lượt tạo các viewbag để lấy list sản phẩm từ cơ sở dữ liệu
			//List Diện điện thoại mới nhất
			//var lstDTM = db.SANPHAMs.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
			var lstDTM = db.SANPHAMs.Where(n => n.MaLoaiSP == 1 && n.DaXoa == false);
			//Gán vào ViewBag
			ViewBag.ListDTM = lstDTM;

			//List LapTop mới nhất
			//var lstLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
			var lstLT = db.SANPHAMs.Where(n => n.MaLoaiSP == 3 && n.DaXoa == false);
			//Gán vào ViewBag
			ViewBag.ListLTM = lstLT;

			//List Máy tính bảng mới nhất
			//var lstMTB = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
			var lstMTB = db.SANPHAMs.Where(n => n.MaLoaiSP == 2 && n.DaXoa == false);
			//Gán vào ViewBag
			ViewBag.ListMTBM = lstMTB;

			return View();
		}

		public ActionResult MenuPartial()
		{
			//Truy vấn lấy về 1 list các sản phẩm
			var lstSP = db.SANPHAMs;
			return PartialView(lstSP);
		}
		[HttpGet]
		public ActionResult DangKy()
		{
			ViewBag.CauHoi = new SelectList(LoadCauHoi());

			return View();
		}

		[HttpPost, CaptchaMvc.Attributes.CaptchaVerify("Captcha is not valid")]
		public ActionResult DangKy(NGUOIDUNG tv, FormCollection f)
		{
			ViewBag.CauHoi = new SelectList(LoadCauHoi());
			//Kiểm tra captcha hợp lệ
			if (this.IsCaptchaValid("Captcha is not valid"))
			{
				if (ModelState.IsValid)
				{
					ViewBag.ThongBao = "Thêm thành công";
					//Thêm khách hàng vào csdl
					db.NGUOIDUNGs.Add(tv);
					db.SaveChanges();
				}
				else
				{
					ViewBag.ThongBao = "Thêm thất bại";
				}
				return View();
			}
			TempData["Message"] = "Message: blahblah";
			ViewBag.ThongBao = "Sai mã captcha";


			return View();
		}
		[HttpGet]
		public ActionResult DangKy1()
		{
			return View();
		}
		[HttpPost]
		public ActionResult DangKy1(NGUOIDUNG tv)
		{
			return View();
		}
		//Load câu hỏi bí mật
		public List<string> LoadCauHoi()
		{
			List<string> lstCauHoi = new List<string>();
			lstCauHoi.Add("Con vật mà bạn yêu thích?");
			lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
			lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
			return lstCauHoi;
		}

		//Xây dựng action đăng nhập
		[HttpPost]
		public ActionResult DangNhap(FormCollection f)
		{
			////Kiểm tra tên đăng nhập và mật khẩu
			//string sTaiKhoan = f["txtTenDangNhap"].ToString();
			//string sMatKhau = f["txtMatKhau"].ToString();
			//NGUOIDUNG tv = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
			//if (tv != null)
			//{
			//    Session["TaiKhoan"] = tv;
			//    return Content("<script>window.location.reload();</script>");
			//}
			//return Content("Tài khoản hoặc mật khẩu không đúng!");
			string taikhoan = f["txtTenDangNhap"].ToString();
			string matkhau = f["txtMatKhau"].ToString();
			//Truy vấn kiểm tra đăng nhập lấy thông tin thành viên
			NGUOIDUNG tv = db.NGUOIDUNGs.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
			if (tv != null)
			{
				////Lấy ra list quyền của thành viên tương ứng với loại thành viên
				//var lstQuyen = db.LOAINGUOIDUNG_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
				////Duyệt list quyền
				//string Quyen = "";
				//if (lstQuyen.Count() != 0)
				//{ 
				//    foreach (var item in lstQuyen)
				//    {
				//        Quyen += item.Quyen.MaQuyen + ",";
				//    }
				//    Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu ","
				//    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
				//    Session["TaiKhoan"] = tv;
				//   return Content("<script>window.location.reload();</script>");
				//}
			}
			return Content("Tài khoản hoặc mật khẩu không đúng!");

		}
		public void PhanQuyen(string TaiKhoan, string Quyen)
		{
			FormsAuthentication.Initialize();
			var ticket = new FormsAuthenticationTicket(1,
																		TaiKhoan, //user
																		DateTime.Now, //Thời gian bắt đầu
																		DateTime.Now.AddHours(3), //Thời gian kết thúc
																		false, //Ghi nhớ?
																		Quyen, // "DangKy,QuanLyDonHang,QuanLySANPHAM"
																		FormsAuthentication.FormsCookiePath);

			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
			if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
			Response.Cookies.Add(cookie);
		}
		//Tạo trang ngăn chặn quyền truy cập
		public ActionResult LoiPhanQuyen()
		{

			return View();
		}
		public ActionResult DangXuat()
		{
			Session["TaiKhoan"] = null;
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}
	}
}