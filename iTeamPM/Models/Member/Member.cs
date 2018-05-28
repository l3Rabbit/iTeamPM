using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;

namespace iTeamPM.Models.Member
{
	public class Member
	{
		public dynamic ReadMember(string text, int skip, int take, ref int total)
		{
			dynamic output = new { };

			using (var db = new DataContext())
			{
				var data = db.iteam_user.Where(z => 1 == 1);

				var text_arr = (!string.IsNullOrEmpty(text)) ? text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x)).Take(3).ToList() : new List<string> { };
				if (text_arr.Count > 0)
				{
					text_arr.ForEach(x => data = data.Where(z => z.name_th.Contains(x)));
				}

				total = data.Count();
				var dataTemplate = data.ToList();
				var data2 = dataTemplate.Skip(skip).Take(take).ToList();
				output = data2;

			}

			return output;
		}

		public dynamic CreateBy(int? user_id)
		{
			dynamic output = new { };

			using (var db = new DataContext())
			{
				var createData = db.iteam_user.Where(x => x.user_id == user_id).FirstOrDefault();
				output = createData;
			}

			return output;
		}

		public void DeleteMember(Models.DataModels.iteam_user m, ref string error)
		{
			using (var db = new DataContext())
			{
				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						var user_id = m?.user_id;

						db.iteam_user.RemoveRange(db.iteam_user.Where(x => x.user_id == user_id));
						db.SaveChanges();

						transaction.Commit();
					}
					catch (DbEntityValidationException ex)
					{
						transaction.Rollback();
						error = ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						error = ex.Message;
						if (ex.InnerException != null)
						{
							error = ex.InnerException.GetBaseException().Message;
						}
					}
				}
			}
		}

	}
}