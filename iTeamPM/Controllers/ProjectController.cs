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
	public class ProjectController : Controller
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

		// GET: Project
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Project_List()
		{
			return View();
		}

		public ActionResult Project_Detail()
		{
			return View();
		}

		public ActionResult Tasks_Detail()
		{
			return View();
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

		public ActionResult ReadGroup()
		{
			var readGroup = new Models.Project.Project(auth).ReadGroup();
			var output = new
			{
				data = readGroup
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult ReadProject(string text, int skip = 0, int take = 6)
		{
			var total = 0;
			var data = new Models.Project.Project(auth).ReadProject(text, skip, take, ref total);
			return Content(JsonConvert.SerializeObject(new { data, total }), "application/json");
		}

		public ActionResult ReadComment(int? task_id)
		{
			var ReadComment = new Models.Project.Project(auth).ReadComment(task_id);
			var output = new
			{
				data = ReadComment
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult ReadTasks(int? project_id, string text, int skip = 0, int take = 6)
		{
			var total = 0;
			var data = new Models.Project.Project(auth).ReadTask(project_id, text, skip, take, ref total);
			return Content(JsonConvert.SerializeObject(new { data, total }), "application/json");
		}

		public ActionResult ReadDate(int? project_id)
		{
			var data = new Models.Project.Project(auth).ReadDate(project_id);
			return Content(JsonConvert.SerializeObject(data), "application/json");
		}

		public ActionResult ReadTasksDetail(int? task_id)
		{
			var readTasks = new Models.Project.Project(auth).ReadTaskDetail(task_id);
			var output = new
			{
				data = readTasks
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult ReadTaskList(int? task_id)
		{
			var readTaskList = new Models.Project.Project(auth).ReadTaskList(task_id);
			var output = new
			{
				data = readTaskList
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult NewProject()
		{
			string error = "";

			var dtl = new DataTools();
			var header = null as Models.DataModels.iteam_project;
			var detail = null as List<Models.DataModels.iteam_project_user>;
			var history = null as List<Models.DataModels.iteam_history_project>;


			var tmp = new
			{
				header = new Dictionary<string, object>(),
				detail = new List<Dictionary<string, object>>(),
                history = new List<Dictionary<string, object>>(),
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_project()).FirstOrDefault() as Models.DataModels.iteam_project;
				detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_project_user()).Select(x => x as Models.DataModels.iteam_project_user).ToList();
                history = dtl.DictionaryToModel(tmp?.history, new Models.DataModels.iteam_history_project()).Select(x => x as Models.DataModels.iteam_history_project).ToList();

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).NewProject(header, detail, history, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = new
				{
					header,
					detail,
                    history
                }
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult NewTask()
		{
			string error = "";


			var dtl = new DataTools();
			var header = null as Models.DataModels.iteam_task;
			var detail = null as List<Models.DataModels.iteam_task_user>;

			var tmp = new
			{
				header = new Dictionary<string, object>(),
				detail = new List<Dictionary<string, object>>(),
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_task()).FirstOrDefault() as Models.DataModels.iteam_task;
				detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_task_user()).Select(x => x as Models.DataModels.iteam_task_user).ToList();

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).NewTask(header, detail, ref error);
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

		public ActionResult NewListTask()
		{
			string error = "";

			var dtl = new DataTools();

			var listArray = null as List<Models.DataModels.iteam_task_lists>;

			var tmp = new
			{
				listArray = new List<Dictionary<string, object>>()
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				listArray = dtl.DictionaryToModel(tmp?.listArray, new Models.DataModels.iteam_task_lists()).Select(x => x as Models.DataModels.iteam_task_lists).ToList();
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).NewListTask(listArray, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = listArray
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult EditTasks(Models.DataModels.iteam_task m)
		{
			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_task();

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
				new Models.Project.Project(auth).EditTasks(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult UpdateListTask()
		{
			string error = "";

			var dtl = new DataTools();

			var listArray = null as List<Models.DataModels.iteam_task_lists>;

			var tmp = new
			{
				listArray = new List<Dictionary<string, object>>()
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				listArray = dtl.DictionaryToModel(tmp?.listArray, new Models.DataModels.iteam_task_lists()).Select(x => x as Models.DataModels.iteam_task_lists).ToList();
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).UpdateTasksList(listArray, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = listArray
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult UpdateStatus()
		{
			string error = "";

			var dtl = new DataTools();

			var m = null as Models.DataModels.iteam_task;

			var tmp = new Dictionary<string, object>();

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_task()).FirstOrDefault() as Models.DataModels.iteam_task;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).UpdateStatus(m, ref error);
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

			var m = null as Models.DataModels.iteam_task_lists;

			var tmp = new Dictionary<string, object>();

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_task_lists()).FirstOrDefault() as Models.DataModels.iteam_task_lists;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).ChangeStatus(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");

		}

		public ActionResult SaveStatus()
		{
			string error = "";

			var dtl = new DataTools();

			var m = null as Models.DataModels.iteam_project;

			var tmp = new Dictionary<string, object>();

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_project()).FirstOrDefault() as Models.DataModels.iteam_project;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).SaveStatus(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");

		}

		public ActionResult DeleteTaskList()
		{
			string error = "";

			var dtl = new DataTools();

			var m = null as Models.DataModels.iteam_task_lists;

			var tmp = new Dictionary<string, object>();

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_task_lists()).FirstOrDefault() as Models.DataModels.iteam_task_lists;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).DeleteTaskList(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult NewComment()
		{
			string error = "";


			var dtl = new DataTools();
			var header = null as Models.DataModels.iteam_comment;

			var tmp = new
			{
				header = new Dictionary<string, object>(),
				//detail = new List<Dictionary<string, object>>(),
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_comment()).FirstOrDefault() as Models.DataModels.iteam_comment;
				//detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_task_user()).Select(x => x as Models.DataModels.iteam_task_user).ToList();

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).NewComment(header, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = new
				{
					header,
					//detail
				}
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult DeleteProject()
		{
			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_project();

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
				new Models.Project.Project(auth).DeleteProject(tmp, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = tmp
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult DeleteTask()
		{
			string error = "";

			var dtl = new DataTools();

			var tmp = new Models.DataModels.iteam_task();

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
				new Models.Project.Project(auth).DeleteTask(tmp, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = tmp
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult DeleteComment()
		{
			string error = "";

			var dtl = new DataTools();


			var m = null as Models.DataModels.iteam_comment;

			var tmp = new Dictionary<string, object>();

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				m = dtl.DictionaryToModel(tmp, new Models.DataModels.iteam_comment()).FirstOrDefault() as Models.DataModels.iteam_comment;

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).DeleteComment(m, ref error);
			}

			var output = new
			{
				success = string.IsNullOrEmpty(error?.Trim()),
				error = error?.Trim(),
				data = m
			};

			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

		public ActionResult EditProject()
		{
			string error = "";

			var dtl = new DataTools();
			var header = null as Models.DataModels.iteam_project;
			var detail = null as List<Models.DataModels.iteam_project_user>;
			var tmp = new
			{
				header = new Dictionary<string, object>(),
				detail = new List<Dictionary<string, object>>(),
			};

			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				header = dtl.DictionaryToModel(tmp?.header, new Models.DataModels.iteam_project()).FirstOrDefault() as Models.DataModels.iteam_project;
				detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_project_user()).Select(x => x as Models.DataModels.iteam_project_user).ToList();

			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).EditProject(header, detail, ref error);
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

		public ActionResult DeleteProjectGroup()
		{

			string error = "";

			var dtl = new DataTools();

			var detail = null as List<Models.DataModels.iteam_project_user>;

			var tmp = new
			{
				detail = new List<Dictionary<string, object>>(),
			};


			try
			{
				var json_string = dtl.JsonRequest();
				tmp = JsonConvert.DeserializeAnonymousType(json_string, tmp);
				detail = dtl.DictionaryToModel(tmp?.detail, new Models.DataModels.iteam_project_user()).Select(x => x as Models.DataModels.iteam_project_user).ToList();
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			if (string.IsNullOrEmpty(error))
			{
				new Models.Project.Project(auth).DeleteProjectGroup(detail, ref error);
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

		// GET : Upload

		[HttpPost]
		public ActionResult UploadFile(HttpPostedFileBase file, Models.DataModels.iteam_upload m)
		{

			string error = "";

			using (var db = new DataContext())
			{
				db.ExecuteTransaction(() =>
				{
					if (file.ContentLength > 0)
					{
						string _FileName = Path.GetFileName(file.FileName);
						var new_name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + "." + Path.GetExtension(file.FileName).Replace(".", "");
						var path = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}

						var path_true = path + "\\" + new_name;

						if (m != null)
						{
							var task_id = m?.task_id;
							m.itemno = (db.iteam_upload.Where(x => x.task_id == task_id).Select(s => s.itemno).Max() ?? 0) + 1;
							m.path_file = path_true;
							m.file_name = _FileName;
							m.add_user = auth.user_id;
							m.add_dt = DateTime.Now;
							db.iteam_upload.Add(m);
							db.SaveChanges();
							file.SaveAs(path_true);
						}
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

		public ActionResult Show(string path)
		{
			if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
			{
				return new HttpStatusCodeResult(404, "File Not Found in iTeam System");
			}
			return File(path, MimeMapping.GetMimeMapping(path));
		}

		public ActionResult Download(string path)
		{
			if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
			{
				return new HttpStatusCodeResult(404, "File Not Found in iTeam System");
			}
			return File(path, MimeMapping.GetMimeMapping(path), System.IO.Path.GetFileName(path));
		}

		public ActionResult ReadFile(int? task_id)
		{
			var tmp = new Models.Project.Project(auth).ReadFile(task_id);
			var output = new
			{
				data = tmp
			};
			return Content(JsonConvert.SerializeObject(output), "application/json");
		}

	}
}