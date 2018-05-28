using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MangoWebPool.Functions;
using Newtonsoft.Json;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;

namespace iTeamPM.Controllers
{
    public class CommunityController : Controller
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

        public ActionResult Community()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(HttpPostedFileBase file, string form, Models.DataModels.iteam_upload_pic pic)
        {
            string error = "";

            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var dtl = new DataTools();
                    var m = null as Models.DataModels.iteam_news;
                    var tmp = new Dictionary<string, object>();

                    try
                    {
                        var json_string = form;
                        tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                        m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_news()).FirstOrDefault() as Models.DataModels.iteam_news;
                    } catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    var news_name = m?.news_name;
                    var news_des = m?.news_des;

                    if (string.IsNullOrEmpty(news_name) && string.IsNullOrEmpty(news_des))
                    {
                        throw new Exception("โปรดกรอกข้อความก่อนส่ง");
                    }

                    m.add_date = DateTime.Now;
                    m.add_user = auth.user_id;
                    m = db.iteam_news.Add(m);
                    db.SaveChanges();
                    
                    if (file != null)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        var new_name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "." + Path.GetExtension(file.FileName).Replace(".", "");
                        var path = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var path_true = path + "\\" + new_name;

                        var new_id = m?.news_id;
                        pic.itemno = (db.iteam_upload_pic.Where(x => x.news_id == new_id).Select(s => s.itemno).Max() ?? 0) + 1;
                        pic.path_file = path_true;
                        pic.file_name = _FileName;
                        pic.add_user = auth.user_id;
                        pic.add_dt = DateTime.Now;
                        pic.news_id = new_id;
                        db.iteam_upload_pic.Add(pic);
                        db.SaveChanges();
                        file.SaveAs(path_true);
                    }


                }, ref error);
            }


            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        public ActionResult DeleteNews()
        {
            string error = "";

            var dtl = new DataTools();


            var m = null as Models.DataModels.iteam_news;

            var tmp = new Dictionary<string, object>();

            try
            {
                var json_string = dtl.JsonRequest();
                tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
                m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_news()).FirstOrDefault() as Models.DataModels.iteam_news;

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (string.IsNullOrEmpty(error))
            {
                new Models.Community.Community(auth).DeleteNews(m, ref error);
            }

            var output = new
            {
                success = string.IsNullOrEmpty(error?.Trim()),
                error = error?.Trim(),
                data = m
            };

            return Content(JsonConvert.SerializeObject(output), "application/json");
        }


        public ActionResult ReadNews()
        {
            var ReadNews = new Models.Community.Community(auth).ReadNews();
            var output = new
            {
                data = ReadNews
            };
            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

        // Community Page2 ------

        public ActionResult ViewCommunity()
        {
            return View();
        }

        public ActionResult ReadNewsDetail(int? news_id)
        {
            var ReadNews = new Models.Community.Community(auth).ReadNewsDetail(news_id);
            var output = new
            {
                data = ReadNews
            };
            return Content(JsonConvert.SerializeObject(output), "application/json");
        }

    }
}