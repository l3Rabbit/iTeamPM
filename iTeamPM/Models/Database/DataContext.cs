using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;
using iTeamPM.Models.DataModels;
using System.Data.Entity.Validation;


namespace iTeamPM.Models.Database
{
	public class DataContext : DbContext
	{
		public DataContext()
		: base(ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString)
		{

		}

		public virtual DbSet<iteam_user> iteam_user { get; set; }
		public virtual DbSet<iteam_group> iteam_group { get; set; }
		public virtual DbSet<iteam_task> iteam_task { get; set; }
		public virtual DbSet<iteam_task_user> iteam_task_user { get; set; }
		public virtual DbSet<iteam_group_user> iteam_group_user { get; set; }
		public virtual DbSet<iteam_project> iteam_project { get; set; }
		public virtual DbSet<iteam_project_user> iteam_project_user { get; set; }
		public virtual DbSet<iteam_comment> iteam_comment { get; set; }
		public virtual DbSet<iteam_task_lists> iteam_task_lists { get; set; }
		public virtual DbSet<iteam_upload> iteam_upload { get; set; }
        public virtual DbSet<iteam_history_project> iteam_history_project { get; set; }
        public virtual DbSet<iteam_news> iteam_news { get; set; }
        public virtual DbSet<iteam_contact> iteam_contact { get; set; }
        public virtual DbSet<iteam_upload_pic> iteam_upload_pic { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

		}

	}

	public static class DataExtensions
	{
		public static void ExecuteTransaction(this DataContext db, Action Do, ref string error_message)
		{
			using (var transaction = db.Database.BeginTransaction())
			{
				try
				{
					Do();

					transaction.Commit();
				}
				catch (DbEntityValidationException ex)
				{
					transaction.Rollback();
					error_message = ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					error_message = ex.Message;
					if (ex.InnerException != null)
					{
						error_message = ex.InnerException.GetBaseException().Message;
					}
				}
			}
		}

	}

}