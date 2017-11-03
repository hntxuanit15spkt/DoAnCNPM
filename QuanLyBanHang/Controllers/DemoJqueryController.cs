using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSiteBanHang.Controllers
{
    public class DemoJqueryController : Controller
    {
        //
        // GET: /DemoJquery/
        public ActionResult Demo()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TestMethodJquery()
        {
            return View();
            
        }
        //[HttpPost]
        //public ActionResult TestMethodJquery(FormCollection f)
        //{
        //    return Content("<input type=\"text\" class=\"test\" />");
        //}
	}
}