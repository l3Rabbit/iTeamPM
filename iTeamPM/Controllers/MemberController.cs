using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;

namespace iTeamPM.Controllers
{
	public class MemberController : Controller
	{

		private Models.Auth auth;
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
				filterContext.Result = RedirectToAction("Login", "Account");
			}
			ViewBag.auth = auth;
		}

		// GET: Member
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Member_List()
		{
			return View();
		}
		public ActionResult Member_Group()
		{
			return View();
		}

		public ActionResult ReadMember(string text, int skip = 0, int take = 10)
		{
			var x = new Models.Member.Member();
			var total = 0;
			var data = x.ReadMember(text, skip, take, ref total);
			return Content(JsonConvert.SerializeObject(new { data, total }), "application/json");
		}

        public ActionResult CreateBy(int? user_id)
		{
			var data = new Models.Member.Member().CreateBy(user_id);
			return Content(JsonConvert.SerializeObject(data), "application/json");
		}

		public ActionResult ViewGroup(int skip = 0, int take = 10)
		{
			var total = 0;
			var data = new Models.Member.MemberGroup(auth).ViewGroup(skip, take, ref total);
			return Content(JsonConvert.SerializeObject(new { data, total }), "application/json");
		}

		public ActionResult NewGroup()
		{
			string error = "";


			var dtl = new DataTools();
			var header = null as Models.DataModels.iteam_group;
			var detail = null as List<Models.DataModels.iteam_group_user>;
			var tmp = new
			{
				header = new Dictionary<string, object>(),
				detail = new List<Dictionary<string, object>>(),
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_group()).FirstOrDefault() as Models.DataModels.iteam_group;
				detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_group_user()).Select(x => x as Models.DataModels.iteam_group_user).ToList();

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Member.MemberGroup(auth).NewGroup(header, detail, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = new
				{
					header,
					detail
				}
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult DeleteMember()
		{

			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_user();


			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Member.Member().DeleteMember(tmp, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = tmp
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");

		}

		public ActionResult DeleteGroup()
		{
			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_group();


			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Member.MemberGroup(auth).DeleteGroup(tmp, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = tmp
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

        public ActionResult DeleteMemberGroup()
        {

            string error = "";

            var dtl = new DataTools();

            var detail = null as List<Models.DataModels.iteam_group_user>;

            var tmp = new
			{
				detail = new List<Dictionary<string, object>>(),
			};


            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_group_user()).Select(x => x as Models.DataModels.iteam_group_user).ToList();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Member.MemberGroup(auth).DeleteMemberGroup(detail, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = new
                {
                    detail
                }
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");

        }

        public ActionResult Editgroup()
		{
            string error = "";

            var dtl = new DataTools();
            var header = null as Models.DataModels.iteam_group;
            var detail = null as List<Models.DataModels.iteam_group_user>;
            var tmp = new
            {
                header = new Dictionary<string, object>(),
                detail = new List<Dictionary<string, object>>(),
            };

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_group()).FirstOrDefault() as Models.DataModels.iteam_group;
                detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_group_user()).Select(x => x as Models.DataModels.iteam_group_user).ToList();

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Member.MemberGroup(auth).Editgroup(header, detail, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = new
                {
                    header,
                    detail
                }
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");
        }


	}
}

