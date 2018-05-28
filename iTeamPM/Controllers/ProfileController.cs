using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;

namespace iTeamPM.Controllers
{
    public class ProfileController : Controller
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

		// GET: Profile
		public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile_User()
        {
            return View();
        }

		public ActionResult ReadProfile(int? user_id)
		{
			var x = new Models.Profile.Profile();
			var data = x.ReadProfile(user_id??0);
			var output = new
			{
				data = data
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult UpdateProfile(Models.DataModels.iteam_user m)
		{
			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_user();

			try
			{
				var json_string = dtl.JsonRequest();
				m = JsonConvert.DeserializeAnonymousType(json_string, tmp);
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Profile.Profile().UpdateProfile(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

	}
}