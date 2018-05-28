using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;


namespace iTeamPM.Controllers
{
    public class CalendarController : Controller
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

		// GET: Calendar
		public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

		public ActionResult ReadEvent(int? project_id)
		{
			var data = new Models.Calendar.Calendar(auth).ReadEvent(project_id);
			return Content(JsonConvert.SerializeObject(data), "application/json");
		}

		public ActionResult ReadProject()
		{
			var data = new Models.Calendar.Calendar(auth).ReadProject();
			var output = new
			{
				data = data
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

    }
}