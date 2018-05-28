using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;

namespace iTeamPM.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult LoginDo()
        {
            var form = new { username = "", password = "" };
            form = Dtl.json_to_object(Dtl.json_request(), form);
            var error = "";
            var model = new Models.Auth();
            model.Login(form.username, form.password, ref error);
            var rtn = new
            {
                success = String.IsNullOrEmpty(error),
                error
            };
            return Content(Dtl.json_stringify(rtn), "application/json");
        }

		public ActionResult LogoutDo()
		{
			var model = new Models.Auth();
			model.Logout();
			return RedirectToAction("Login");
		}

        public ActionResult RegisterDo(Models.DataModels.iteam_user m)
        {
            string error = "";

            var dtl = new DataTools();

            var tmp = new Models.DataModels.iteam_user();

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                m = tmp;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Account.Account().Register(m, ref error);
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