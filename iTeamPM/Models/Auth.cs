using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;
using Newtonsoft.Json;
using MangoWebPool.Functions;

namespace iTeamPM.Models
{
	public class Auth
	{
		public int? user_id { get; set; } = null;
		public string username { get; set; } = null;
		public string path_image { get; set; } = null;
		public string name_th { get; set; } = null;
		public string postion { get; set; } = null;
        public string is_admin { get; set; } = null;
		public bool is_auth { get; set; } = false;

		public void Login(string username, string password, ref string error)
		{
			using (var db = new DataContext())
			{
				try
				{
					if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
					{
						throw new Exception("โปรดกรอกรหัส");
					}

					var u = db.iteam_user.Where(x => x.username == username).FirstOrDefault();

					if (u == null)
					{
						throw new Exception("ไม่พบรหัส");
					}

					if (u.password != password)
					{
						throw new Exception("รหัสไม่ถูกต้อง");
					}

					HttpContext.Current.Response.Cookies.Add(new HttpCookie("iteam_auth")
					{
						Value = new MangoTokenProvider.Token().CreateTokenHex(JsonConvert.SerializeObject(new { salt = new MangoTokenProvider.Password().CreatePassword(8), username, password })),
						Expires = DateTime.Now.AddYears(1)
					});

				}
				catch (Exception ex)
				{
					error = ex.Message;
				}
			}
		}

		public void Logout()
		{
			HttpContext.Current.Response.Cookies.Add(new HttpCookie("iteam_auth")
			{
				Value = "",
				Expires = DateTime.Now.AddYears(-10)
			});
		}

		public void CheckUser()
		{
			var error = "";
			var user = new
			{
				username = "",
				password = ""
			};

			using (var db = new DataContext())
			{
				try
				{
					if (string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["iteam_auth"]?.Value))
					{
						throw new Exception("T");
					}

					var json = "";
					new MangoTokenProvider.Token().CheckTokenHex(HttpContext.Current.Request.Cookies["iteam_auth"]?.Value, out json);
					user = Dtl.json_to_object(json, user);

					if (String.IsNullOrEmpty(user.username) || String.IsNullOrEmpty(user.password))
					{
						throw new Exception("โปรดกรอกรหัส");
					}

					var u = db.iteam_user.Where(x => x.username == user.username).FirstOrDefault();

					if (u == null)
					{
						throw new Exception("ไม่พบรหัส");
					}

					if (u.password != user.password)
					{
						throw new Exception("รหัสไม่ถูกต้อง");
					}

					this.username = u.username;
					this.user_id = u.user_id;
					this.path_image = u.path_image;
					this.name_th = u.name_th;
					this.postion = u.postion;
                    this.is_admin = u.is_admin;
					this.is_auth = true;

				}
				catch (Exception ex)
				{
					error = ex.Message;
				}
			}
		}

	}
}