using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;

namespace iTeamPM.Controllers
{
    public class APIController : Controller
    {

		Models.Auth auth;
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			auth = new Models.Auth();
			auth.CheckUser();
			if (auth.is_auth)
			{
				base.OnActionExecuting(filterContext);
			}
			else
			{
				//filterContext.Result = new HttpStatusCodeResult(403);
				filterContext.Result = RedirectToAction("Login", "Account");

			}
			ViewBag.auth = auth;
		}

		// GET: API
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult MapView()
		{
			return View();
		}

    }
}