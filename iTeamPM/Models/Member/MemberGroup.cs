using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;

namespace iTeamPM.Models.Member
{

    public class MemberGroup
    {
        private Models.Auth auth;
        public MemberGroup(Models.Auth auth)
        {
            this.auth = auth;

        }

        public dynamic ViewGroup(int skip, int take, ref int total)
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {

				var data = (from a in db.iteam_group
							let memberData = (from b in db.iteam_group_user
											  join c1 in db.iteam_user on new { b.user_id } equals new { c1.user_id } into c2
											  from c in c2.DefaultIfEmpty()

											  where a.group_id == b.group_id
											  select new
											  {
												  b.group_id,
												  b.user_id,
												  c.username,
												  c.path_image,
												  c.name_th
											  }).ToList()

							let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.username).FirstOrDefault()
							let user_id = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.user_id).FirstOrDefault()
							where memberData.Select(x => x.user_id).Contains(auth.user_id)

                            select new
                            {
                                a.group_id,
                                a.group_name,
                                a.group_description,
                                a.add_user,
								user_id,
								username,
                                a.add_dt,
                                a.edit_user,
                                a.edit_dt,
								memberData
                            }
					
                ).ToList();

				total = data.Count();
				var dataTemplate = data.Skip(skip).Take(take);

				output = dataTemplate;

            }
            return output;
        }

        public void NewGroup(DataModels.iteam_group header, List<DataModels.iteam_group_user> detail, ref string error)
        {
            using (var db = new DataContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var group_name = header?.group_name.Trim();

                        if (string.IsNullOrEmpty(group_name))
                        {
                            throw new Exception("Error : กรุณากรอก Group Name");
                        }

                        header.group_id = (db.iteam_group.Select(x => x.group_id).Max() ?? 0) + 1;
                        header.add_dt = DateTime.Now;
                        header.add_user = auth.user_id;

                        db.iteam_group.Add(header);
                        db.SaveChanges();

                        if (detail != null && detail.Count() > 0)
                        {
                            foreach (var x in detail)
                            {
                                x.group_id = header.group_id;
                            }
                            db.iteam_group_user.AddRange(detail);
                            db.SaveChanges();
                        }

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

		public void DeleteGroup(DataModels.iteam_group m,ref string error)
		{
			using (var db = new DataContext())
			{
				db.ExecuteTransaction(() =>
				{
					var group_id = m?.group_id;

					db.iteam_group.RemoveRange(db.iteam_group.Where(x => x.group_id == group_id));
					db.iteam_group_user.RemoveRange(db.iteam_group_user.Where(x => x.group_id == group_id));

					db.SaveChanges();

				}, ref error);
			}
		}

        public void DeleteMemberGroup(List<DataModels.iteam_group_user> detail, ref string error)
        {
            using(var db = new DataContext()){
                
                    
                            foreach (var F in detail)
                            {
                                db.ExecuteTransaction(() =>
                                {
                                    var group_id = F?.group_id;
                                    var user_id = F?.user_id;

                                    db.iteam_group_user.RemoveRange(db.iteam_group_user.Where(x => x.group_id == group_id && x.user_id == user_id));

                                    db.SaveChanges();

                                }, ref error);
                            }
                        
                    
                
                
            }
        }

        public void Editgroup(DataModels.iteam_group header, List<DataModels.iteam_group_user> detail, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var group_id = header?.group_id;
                    var group_name = header?.group_name.Trim();
                    var group_description = header?.group_description;

                    if (string.IsNullOrEmpty(group_name) )
                    {
                        throw new Exception("โปรดกรอกชื่อให้ถูกต้อง");
                    }

                    var data_db = db.iteam_group.Where(x => x.group_id == group_id).FirstOrDefault();

                    if (data_db != null)
                    {
                        data_db.group_name = group_name;

                        data_db.group_description = group_description;

                        db.SaveChanges();

                        if (detail != null && detail.Count() > 0)
                        {
                            foreach (var x in detail)
                            {
                                x.group_id = header.group_id;
                            }
                            db.iteam_group_user.AddRange(detail);
                            db.SaveChanges();
                        }

                    }
                }, ref error);
            }
        }

    }
}