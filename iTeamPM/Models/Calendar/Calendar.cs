using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Data.Entity.Validation;
using System.Data.Entity;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;

namespace iTeamPM.Models.Calendar
{
	public class Calendar
	{
		private Models.Auth auth;
		public Calendar(Models.Auth auth)
		{
			this.auth = auth;

		}

		public dynamic ReadProject()
		{
			dynamic output = new { };

			using (var db = new DataContext())
			{
				var data = (from a in db.iteam_project

							let memberProject = (from b in db.iteam_project_user
												 join b1 in db.iteam_user on new { b.user_id } equals new { b1.user_id } into b2
												 from c in b2.DefaultIfEmpty()

												 where a.project_id == b.project_id
												 select new
												 {
													 b.user_id,
													 b.project_id,
													 c.username,
													 c.name_th,
													 c.path_image
												 }).ToList()

							where memberProject.Select(x => x.user_id).Contains(auth.user_id) || a.add_user == auth.user_id

							select new
							{
								a.project_id,
								a.project_name,
							}).ToList();

				output = data;

			}

			return output;
		}

		public dynamic ReadEvent(int? project_id)
		{
			dynamic output = new { };
			using (var db = new DataContext())
			{
				var data = (from a in db.iteam_task
							join b1 in db.iteam_project on new { a.project_id } equals new { b1.project_id } into b2
							from b in b2.DefaultIfEmpty()

							where a.project_id == project_id

							select a
							
							).ToList().Select(s => new {
								s.task_id,
								title = s.tasks_name,
								start = s.date_start?.ToString("yyyy-MM-dd") ?? "",
								end = s.date_end?.ToString("yyyy-MM-dd") ?? "",
								color = "Colors.brandSuccess",
							}).ToList();
				output = data;
			}

			return output;
		}

	}
}