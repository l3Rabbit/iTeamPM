using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;

namespace iTeamPM.Controllers
{
    public class HomeController : Controller
    {
		Models.Auth auth;
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			auth = new Models.Auth();
			auth.CheckUser();
			if(auth.is_auth)
			{
				base.OnActionExecuting(filterContext);
			}
			else
			{
				filterContext.Result = RedirectToAction("Login", "Account");
			}
			ViewBag.auth = auth;
		}

		// GET: Home
		public ActionResult Index(Models.Auth auth)
        {
			this.auth = auth;
			return View();
        }

		public ActionResult ReadNotifications()
		{
			var total_project = 0;
			var total_tasks = 0;
			var data = new Models.Home.Home(auth).ReadNotifications(ref total_project, ref total_tasks);
			return Content(JsonConvert.SerializeObject(data), "application/json");
		}

        public ActionResult UpdateReadProject(int? project_id)
        {
            string error = "";

            if (string.IsNullOrEmpty(error))
            {
                new Models.Home.Home(auth).UpdateReadProject(project_id, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = project_id,
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult ReadProject()
        {
            var data = new Models.Home.Home(auth).ReadProject();
			var output = new
			{
				data = data
			};
            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult ReadTasks()
        {
            var data = new Models.Home.Home(auth).ReadTasks();
			var output = new
			{
				data = data
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
        }

		public ActionResult ViewGroup()
		{
			var data = new Models.Home.Home(auth).ViewGroup();
			var output = new
			{
				data = data
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult ReadUserProject(int? project_id)
		{
			var readUser = new Models.Project.Project(auth).ReadUserProject(project_id);
			var output = new
			{
				data = readUser
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

	}
}