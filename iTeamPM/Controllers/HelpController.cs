using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;



namespace iTeamPM.Controllers
{
    public class HelpController : Controller
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
                filterContext.Result = RedirectToAction("Login", "Account");
            }
            ViewBag.auth = auth;
        }
        // GET: Help
        public ActionResult AboutiTeam()
        {
            return View();
        }
        public ActionResult ContactAdd()
        {
            string error = "";


            var dtl = new DataTools();
            var data = null as Models.DataModels.iteam_contact;

            var tmp = new
            {
                data = new Dictionary<string, object>(),
            };

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                data = dtl.DictionaryToModel(tmp?.data, new Models.DataModels.iteam_contact()).FirstOrDefault() as Models.DataModels.iteam_contact;

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Contact.Contact(auth).ContactAdd(data, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = new
                {
                    data,
                }
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult ViewContact()
        {
            return View();
        }

        public ActionResult ContactOpen()
        {
            return View();
        }
        public ActionResult ReadContact(string text, int skip = 0, int take = 10)
        {
            var total = 0;
            var data = new Models.Contact.Contact(auth).ReadContact(text, skip, take, ref total);
            return Content(JsonConvert.SerializeObject(new { data, total }), "application/json");

            //var ReadContact = new Models.Contact.Contact(auth).ReadContact();
            //var output = new
            //{
            //    data = ReadContact
            //};
            //return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult ContactReadDetail(int? contact_id)
        {
            var ReadContact = new Models.Contact.Contact(auth).ContactReadDetail(contact_id);
            var output = new
            {
                data = ReadContact
            };
            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult ChangeRead()
        {
            string error = "";

            var dtl = new DataTools();

            var m = null as Models.DataModels.iteam_contact;

            var tmp = new Dictionary<string, object>();

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_contact()).FirstOrDefault() as Models.DataModels.iteam_contact;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Contact.Contact(auth).ChangeRead(m, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = m
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");

        }

        public ActionResult ChangeStatus()
        {
            string error = "";

            var dtl = new DataTools();

            var m = null as Models.DataModels.iteam_contact;

            var tmp = new Dictionary<string, object>();

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_contact()).FirstOrDefault() as Models.DataModels.iteam_contact;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Contact.Contact(auth).ChangeStatus(m, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = m
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");

        }

        public ActionResult ContactDelete()
        {
            string error = "";

            var dtl = new DataTools();

            var m = null as Models.DataModels.iteam_contact;

            var tmp = new Dictionary<string, object>();

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_contact()).FirstOrDefault() as Models.DataModels.iteam_contact;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Contact.Contact(auth).ContactDelete(m, ref error);
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