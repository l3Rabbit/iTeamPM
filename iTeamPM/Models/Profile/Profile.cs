using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;

namespace iTeamPM.Models.Profile
{
	public class Profile
	{
		public dynamic ReadProfile(int? user_id)
		{
			dynamic output = new { };

			using (var db = new DataContext())
			{
				var data = db.iteam_user.Where(x => x.user_id == user_id).FirstOrDefault();

				output = data;
			}

			return output;
		}

		public void UpdateProfile(Models.DataModels.iteam_user m, ref string error)
		{
			using (var db = new DataContext())
			{
				db.ExecuteTransaction(() =>
				{
					var user_id = m?.user_id;
					var username = m?.username.Trim();
					var name_th = m?.name_th;
					var email = m?.email.Trim();
					var position = m?.postion;
					var description = m?.description;
					var phone = m?.phone.Trim();
					var line_id = m?.line_id.Trim();
                    var path_image = m?.path_image?.Trim();


                    if (string.IsNullOrEmpty(name_th) || string.IsNullOrEmpty(email))
					{
						throw new Exception("โปรดกรอกชื่อหรืออีเมลให้ถูกต้อง");
					}

					var data_db = db.iteam_user.Where(x => x.user_id == user_id).FirstOrDefault();

					if(data_db != null)
					{
						data_db.name_th = name_th;
						data_db.email = email;
						data_db.postion = position;
						data_db.description = description;
						data_db.phone = phone;
						data_db.line_id = line_id;
                        data_db.path_image = path_image;

                        db.SaveChanges();
                    }

				}, ref error);
			}
		}

	}
}