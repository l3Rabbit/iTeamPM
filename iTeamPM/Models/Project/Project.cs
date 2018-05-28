using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;

namespace iTeamPM.Models.Project
{
    public class Project
    {
        private Models.Auth auth;
        public Project(Models.Auth auth)
        {
            this.auth = auth;

        }

        // Database : Project
        public dynamic ReadProject(string text, int skip, int take, ref int total)
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

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()
                            where memberProject.Select(x => x.user_id).Contains(auth.user_id) || a.add_user == auth.user_id

                            select new
                            {
                                a.project_id,
                                a.project_name,
                                a.project_des,
                                a.start_project,
                                a.end_project,
                                a.add_user,
                                a.add_dt,
                                a.tasks_complete,
                                a.tasks_count,
                                a.status_project,
                                overdue = DbFunctions.DiffDays(a.end_project, a.start_project) * -1,
                                username,
                                memberProject
                            });

                var text_arr = (!string.IsNullOrEmpty(text)) ? text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x)).Take(3).ToList() : new List<string> { };
                if (text_arr.Count > 0)
                {
                    text_arr.ForEach(x => data = data.Where(z => z.project_name.Contains(x) || z.username.Contains(x)));
                }

                total = data.Count();
                var dataTemplate = data.ToList();
                var final = dataTemplate.Skip(skip).Take(take).ToList();
                output = final;

            }

            return output;
        }

        public dynamic ReadUserProject(int? project_id)
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_project

                            join b1 in db.iteam_project_user on new { a.project_id } equals new { b1.project_id } into b2
                            from b in b2.DefaultIfEmpty()

                            join c1 in db.iteam_user on new { b.user_id } equals new { c1.user_id } into c2
                            from c in c2.DefaultIfEmpty()

                            where a.project_id == project_id

                            select new
                            {
                                a.project_id,
                                a.project_name,
                                c.path_image,
                                c.user_id,
                                c.name_th,
                                c.postion
                            }).OrderBy(x => x.name_th).ThenBy(t => t.user_id).ToList();

                output = data;
            }
            return output;
        }

        public dynamic ReadGroup()
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_group

                            let memberGroup = (from b in db.iteam_group_user
                                               join b1 in db.iteam_user on new { b.user_id } equals new { b1.user_id } into b2
                                               from c in b2.DefaultIfEmpty()

                                               where a.group_id == b.group_id

                                               select new
                                               {
                                                   c.user_id,
                                                   c.name_th,
                                                   c.path_image,
                                                   c.postion
                                               }).ToList()

                            select new
                            {
                                a.group_id,
                                a.group_name,
                                memberGroup
                            }).ToList();

                output = data;

            }

            return output;

        }

        public dynamic ReadDate(int? project_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from d in db.iteam_project
                            join g1 in db.iteam_task on new { d.project_id } equals new { g1.project_id } into g2
                            from p in g2.DefaultIfEmpty()

                            where d.project_id == project_id
                            select d

                     ).ToList().Select(s => new
                     {
                         date_start = s.start_project?.ToString("dd/MM/yyyy") ?? "",
                         range_start = s.start_project?.ToString("yyyy-MM-dd") ?? "",
                         range_end = s.end_project?.ToString("yyyy-MM-dd") ?? "",
                     }).FirstOrDefault();

                output = data;
            }

            return output;
        }

        public void SaveStatus(iteam_project m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var project_id = m?.project_id;

                    var data = db.iteam_project.Where(x => x.project_id == project_id).FirstOrDefault();

                    if (data != null)
                    {
                        data.status_project = "Y";
                        db.SaveChanges();
                    }

                }, ref error);
            }
        }

        public void NewProject(DataModels.iteam_project header, List<DataModels.iteam_project_user> detail, List<DataModels.iteam_history_project> history, ref string error)
        {
            using (var db = new DataContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var project_name = header?.project_name.Trim();
                        var start_project = header?.start_project;
                        var end_project = header?.end_project;

                        if (string.IsNullOrEmpty(project_name))
                        {
                            throw new Exception("กรุณากรอกชื่อโปรเจค");
                        }

                        if (start_project == null || end_project == null)
                        {
                            throw new Exception("กรุณาเลือกวันเริ่มต้นหรือวันสิ้นสุดของโปรเจค");
                        }

                        header.project_id = (db.iteam_project.Select(x => x.project_id).Max() ?? 0) + 1;
                        header.start_project = start_project?.AddDays(1);
                        header.end_project = end_project?.AddDays(1);
                        header.add_dt = DateTime.Now;
                        header.add_user = auth.user_id;
                        header.status_project = "N";
                        header.tasks_complete = 0;
                        header.tasks_count = 0;

                        db.iteam_project.Add(header);
                        db.SaveChanges();

                        if (detail != null && detail.Count() > 0)
                        {
                            foreach (var x in detail)
                            {
                                x.project_id = header.project_id;
                            }
                            db.iteam_project_user.AddRange(detail);
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("กรุณาเลือกผู้ใช้งานลงในโปรเจค");
                        }

                        if (history != null && history.Count() > 0)
                        {
                            foreach (var x in history)
                            {
                                x.project_id = header.project_id;
                                x.project_name = header.project_name;
                                x.read_notify = "N";
                                x.active = "Y";
                                x.add_user = auth.user_id;
                            }

                            db.iteam_history_project.AddRange(history);
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

        public void EditProject(DataModels.iteam_project header, List<DataModels.iteam_project_user> detail, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var project_id = header?.project_id;
                    var project_name = header?.project_name.Trim();
                    var project_des = header?.project_des;
                    var end_project = header?.end_project;

                    if (string.IsNullOrEmpty(project_name))
                    {
                        throw new Exception("โปรดกรอกชื่อให้ถูกต้อง");
                    }

                    var data_db = db.iteam_project.Where(x => x.project_id == project_id).FirstOrDefault();

                    if (data_db != null)
                    {
                        data_db.project_name = project_name;
                        data_db.project_des = project_des;
                        data_db.end_project = end_project;
                        data_db.edit_dt = DateTime.Now;
                        db.SaveChanges();

                        if (detail != null && detail.Count() > 0)
                        {
                            foreach (var x in detail)
                            {
                                x.project_id = header.project_id;
                            }
                            db.iteam_project_user.AddRange(detail);
                            db.SaveChanges();
                        }

                    }
                }, ref error);
            }
        }

        public void DeleteProject(DataModels.iteam_project m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var project_id = m?.project_id;
                    var project_name = m?.project_name;
                    var tasksDB = db.iteam_task.Where(x => x.project_id == project_id).Select(x => x.task_id).ToList();

                    if (m != null)
                    {
                        db.iteam_project.RemoveRange(db.iteam_project.Where(x => x.project_id == project_id));
                        db.iteam_project_user.RemoveRange(db.iteam_project_user.Where(x => x.project_id == project_id));
                        db.iteam_task.RemoveRange(db.iteam_task.Where(x => x.project_id == project_id));
                        db.iteam_task_user.RemoveRange(db.iteam_task_user.Where(x => tasksDB.Contains(x.task_id)));
                        db.iteam_comment.RemoveRange(db.iteam_comment.Where(x => tasksDB.Contains(x.task_id)));
                        db.iteam_task_lists.RemoveRange(db.iteam_task_lists.Where(x => tasksDB.Contains(x.task_id)));
                        db.SaveChanges();

                        var history_project = db.iteam_history_project.Where(x => x.project_id == project_id).ToList();
                        if (history_project != null)
                        {
                            foreach (var h in history_project)
                            {
                                h.active = "N";
                                h.project_name = project_name;
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new Exception("Error");
                    }


                }, ref error);
            }
        }

        public void DeleteProjectGroup(List<DataModels.iteam_project_user> detail, ref string error)
        {
            using (var db = new DataContext())
            {
                foreach (var F in detail)
                {
                    db.ExecuteTransaction(() =>
                    {
                        var project_id = F?.project_id;
                        var user_id = F?.user_id;

                        db.iteam_project_user.RemoveRange(db.iteam_project_user.Where(x => x.project_id == project_id && x.user_id == user_id));
                        db.SaveChanges();

                    }, ref error);
                }
            }
        }

        // Database : Tasks
        public dynamic ReadTask(int? project_id, string text, int skip, int take, ref int total)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_task

                            let memberTasks = (from b in db.iteam_task_user
                                               join b1 in db.iteam_user on new { b.user_id } equals new { b1.user_id } into b2
                                               from c in b2.DefaultIfEmpty()

                                               where a.task_id == b.task_id
                                               select new
                                               {
                                                   b.user_id,
                                                   b.task_id,
                                                   c.username,
                                                   c.name_th,
                                                   c.path_image
                                               }).ToList()

                            let status_project = (from c in db.iteam_project
                                                  join v1 in db.iteam_task on new { c.project_id } equals new { v1.project_id } into v2
                                                  from e in v2.DefaultIfEmpty()

                                                  where a.project_id == e.project_id
                                                  select c.status_project

                                                  ).FirstOrDefault()

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()

                            let all_list = db.iteam_task_lists.Where(x => x.task_id == a.task_id).Count()
                            let incomplete_list = db.iteam_task_lists.Where(x => x.task_id == a.task_id && x.status == "1").Select(x => x.status).Count()
                            let waiting_list = db.iteam_task_lists.Where(x => x.task_id == a.task_id && x.status == "2").Select(x => x.status).Count()
                            let complete_list = db.iteam_task_lists.Where(x => x.task_id == a.task_id && x.status == "3").Select(x => x.status).Count()

                            where a.project_id == project_id

                            select new
                            {
                                a.project_id,
                                a.task_id,
                                a.tasks_name,
                                a.tasks_description,
                                a.date_start,
                                a.date_end,
                                a.tasks_prioity,
                                a.add_dt,
                                a.add_user,
                                a.status,
                                username,
                                overdue = DbFunctions.DiffDays(a.date_end, a.date_start) * -1,
                                incomplete_list,
                                waiting_list,
                                complete_list,
                                all_list,
                                status_project,
                                memberTasks,
                            });

                var text_arr = (!string.IsNullOrEmpty(text)) ? text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x)).Take(3).ToList() : new List<string> { };
                if (text_arr.Count > 0)
                {
                    text_arr.ForEach(x => data = data.Where(z => z.tasks_name.Contains(x) || z.username.Contains(x)));
                }

                total = data.Count();
                var mainData = data.OrderByDescending(x => x.tasks_prioity);
                var dataTemplate = mainData.Skip(skip).Take(take).ToList();
                output = dataTemplate;
            }

            return output;
        }

        public dynamic ReadTaskDetail(int? task_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_task

                            let memberTasks = (from b in db.iteam_task_user

                                               join b1 in db.iteam_user on new { b.user_id } equals new { b1.user_id } into b2
                                               from c in b2.DefaultIfEmpty()

                                               where a.task_id == b.task_id
                                               select new
                                               {
                                                   b.user_id,
                                                   b.task_id,
                                                   c.username,
                                                   c.name_th,
                                                   c.path_image,
                                                   c.postion
                                               }).ToList()

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()

                            let project_name = (from d in db.iteam_project
                                                where d.project_id == a.project_id
                                                select d.project_name).FirstOrDefault()

                            where a.task_id == task_id

                            select new
                            {
                                a.project_id,
                                project_name,
                                a.task_id,
                                a.tasks_name,
                                a.tasks_description,
                                a.date_start,
                                a.date_end,
                                a.tasks_prioity,
                                a.add_dt,
                                a.add_user,
                                a.status,
                                username,
                                memberTasks,
                            }).FirstOrDefault();

                output = data;
            }

            return output;
        }

        public void NewTask(DataModels.iteam_task header, List<DataModels.iteam_task_user> detail, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var tasks_name = header?.tasks_name;
                    var tasks_prioity = header?.tasks_prioity;
                    var date_start = header?.date_start;
                    var date_end = header?.date_end;

                    if (string.IsNullOrEmpty(tasks_name))
                    {
                        throw new Exception("โปรดกรอกชื่อของระบบงาน");
                    }

                    if (string.IsNullOrEmpty(tasks_prioity))
                    {
                        throw new Exception("โปรดเลือกระดับความสำคัญของระบบงาน");
                    }

                    if (date_start == null || date_end == null)
                    {
                        throw new Exception("โปรดเลือกวันที่เริ้มต้นหรือวันสิ้นสุดของระบบงาน");
                    }

                    var data = (from d in db.iteam_project
                                join g1 in db.iteam_task on new { d.project_id } equals new { g1.project_id } into g2
                                from p in g2.DefaultIfEmpty()

                                where d.project_id == header.project_id
                                select d.end_project

                         ).FirstOrDefault();

                    if (date_end > data)
                    {
                        throw new Exception("วันที่สิ้นสุดของระบบงานไม่สามารถเกินวันสิ้นสุดของโปรเจคได้");
                    }

                    header.task_id = (db.iteam_task.Select(x => x.task_id).Max() ?? 0) + 1;
                    header.status = "N";
                    header.date_start = date_start?.AddDays(1);
                    header.date_end = date_end?.AddDays(1);
                    header.add_dt = DateTime.Now;
                    header.add_user = auth.user_id;
                    db.iteam_task.Add(header);
                    db.SaveChanges();

                    var prj = db.iteam_project.Where(x => x.project_id == header.project_id).FirstOrDefault();
                    prj.tasks_count = db.iteam_task.Where(x => x.project_id == header.project_id).Count();
                    db.SaveChanges();

                    if (detail != null && detail.Count() > 0)
                    {
                        foreach (var x in detail)
                        {
                            x.task_id = header.task_id;
                        }
                        db.iteam_task_user.AddRange(detail);
                        db.SaveChanges();
                    }
                }, ref error);
            }
        }

        public void EditTasks(DataModels.iteam_task m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var tasks_id = m?.task_id;
                    var tasks_name = m?.tasks_name;
                    var tasks_des = m?.tasks_description;
                    var date_start = m?.date_start;
                    var date_end = m?.date_end;

                    var data_db = db.iteam_task.Where(w => w.task_id == tasks_id).FirstOrDefault();

                    if (data_db != null)
                    {
                        data_db.tasks_name = tasks_name;
                        data_db.tasks_description = tasks_des;
                        data_db.date_end = date_end;
                        db.SaveChanges();
                    }

                }, ref error);
            }
        }

        public void DeleteTask(DataModels.iteam_task m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var task_id = m?.task_id;

                    if (m != null)
                    {
                        var prj = db.iteam_project.Where(x => x.project_id == m.project_id).FirstOrDefault();
                        prj.tasks_count = db.iteam_task.Where(x => x.project_id == m.project_id).Count() - 1;
                        db.SaveChanges();

                        db.iteam_task.RemoveRange(db.iteam_task.Where(x => x.task_id == task_id));
                        db.iteam_task_user.RemoveRange(db.iteam_task_user.Where(x => x.task_id == task_id));
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Error");
                    }


                }, ref error);
            }
        }

        public void UpdateStatus(iteam_task m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var task_id = m?.task_id;
                    var project_id = m?.project_id;

                    var data = db.iteam_task.Where(x => x.task_id == task_id && x.project_id == project_id).FirstOrDefault();

                    if (data != null)
                    {
                        data.status = "Y";
                        db.SaveChanges();
                    }

                    var complete = db.iteam_project.Where(x => x.project_id == project_id).FirstOrDefault();
                    complete.tasks_complete = db.iteam_task.Where(x => x.status == "Y" && x.project_id == project_id).Count();
                    db.SaveChanges();

                }, ref error);
            }
        }

        // Database : Tasks list
        public dynamic ReadTaskList(int? task_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_task_lists
                            join b1 in db.iteam_task on new { a.task_id } equals new { b1.task_id } into b
                            from c in b.DefaultIfEmpty()

                            let save_name = db.iteam_user.Where(x => x.user_id == a.save_user).Select(x => x.name_th).FirstOrDefault()

                            where a.task_id == task_id

                            select new
                            {
                                a.task_id,
                                a.itemno,
                                a.active,
                                a.status,
                                a.name_task_list,
                                a.remark_list,
                                a.save_user,
                                a.edit_date,
                                save_name
                            }).ToList();
                output = data;
            }

            return output;
        }

        public void NewListTask(List<DataModels.iteam_task_lists> listArray, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    if (listArray != null && listArray.Count() > 0)
                    {
                        var task_id = listArray.First().task_id;
                        var itemno = (db.iteam_task_lists.Where(x => x.task_id == task_id).Select(s => s.itemno).Max() ?? 0) + 1;
                        foreach (var x in listArray)
                        {
                            if (String.IsNullOrEmpty(x.name_task_list))
                            {
                                throw new Exception("โปรดกรอกข้อมูลให้ครบถ้วน");
                            }
                            x.itemno = itemno++;
                            x.save_user = auth.user_id;
                            x.edit_user = auth.user_id;
                            x.add_dt = DateTime.Now;
                            x.status = "1";
                            x.active = "N";
                        }
                        db.iteam_task_lists.AddRange(listArray);
                        db.SaveChanges();
                    }

                }, ref error);
            }
        }

        public void DeleteTaskList(Models.DataModels.iteam_task_lists m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var tasks_id = m?.task_id;
                    var itemno = m?.itemno;

                    db.iteam_task_lists.RemoveRange(db.iteam_task_lists.Where(w => w.task_id == tasks_id && w.itemno == itemno));
                    db.SaveChanges();

                }, ref error);
            }
        }

        public void UpdateTasksList(List<iteam_task_lists> listArray, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    if (listArray != null && listArray.Count() > 0)
                    {
                        foreach (var x in listArray)
                        {
                            var fee = db.iteam_task_lists.Where(xx => xx.task_id == x.task_id && xx.itemno == x.itemno).First();
                            fee.save_user = auth.user_id;
                            fee.edit_date = DateTime.Now;
                            fee.remark_list = x.remark_list;
                            fee.status = "3";
                            fee.active = x.active;
                        }
                        db.SaveChanges();
                    }

                }, ref error);
            }
        }

        public void ChangeStatus(iteam_task_lists m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var task_id = m?.task_id;
                    var itemno = m?.itemno;

                    var data = db.iteam_task_lists.Where(x => x.task_id == task_id && x.itemno == itemno).FirstOrDefault();

                    if (data != null)
                    {
                        data.remark_list = m?.remark_list;
                        data.active = "N";
                        if (m?.status == "3")
                        {
                            data.status = "1";
                        }
                        else if (m?.status == "2")
                        {
                            data.status = "1";
                        }
                        else
                        {
                            data.status = "2";
                        }
                        db.SaveChanges();
                    }

                }, ref error);
            }
        }

        // Database : File Tasks list
        public dynamic ReadFile(int? task_id)
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_upload
                            join b1 in db.iteam_task on new { a.task_id } equals new { b1.task_id } into b2
                            from b in b2.DefaultIfEmpty()

                            where a.task_id == task_id

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(s => s.name_th).FirstOrDefault()

                            select new
                            {
                                a.task_id,
                                a.file_name,
                                a.path_file,
                                username
                            }).ToList();
                output = data;
            }
            return output;
        }


        // Database : Comment Tasks
        public dynamic ReadComment(int? task_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_comment
                            join comment in db.iteam_task on new { a.task_id } equals new { comment.task_id } into b
                            from c in b.DefaultIfEmpty()

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()
                            let image = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.path_image).FirstOrDefault()

                            where a.task_id == task_id

                            select new
                            {
                                image,
                                username,
                                a.add_user,
                                a.comment,
                                a.add_dt,
                                a.itemno,
                                a.task_id
                            }).ToList();

                output = data;

            }

            return output;
        }

        public void NewComment(DataModels.iteam_comment header, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var comment = header?.comment;
                    var task_id = header?.task_id;

                    if (string.IsNullOrEmpty(comment))
                    {
                        throw new Exception("โปรดกรอกข้อความก่อนส่ง");
                    }

                    header.itemno = (db.iteam_comment.Where(x => x.task_id == task_id).Select(x => x.itemno).Max() ?? 0) + 1;
                    header.add_dt = DateTime.Now;
                    header.add_user = auth.user_id;
                    db.iteam_comment.Add(header);
                    db.SaveChanges();

                }, ref error);
            }
        }

        public void DeleteComment(DataModels.iteam_comment m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {

                    var itemno = m?.itemno;
                    var task_id = m?.task_id;

                    //var data = db.iteam_comment.Where(x => x.itemno == itemno && x.task_id == task_id).ToList();
                    //throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(data));

                    db.iteam_comment.RemoveRange(db.iteam_comment.Where(x => x.itemno == itemno && x.task_id == task_id).ToList());

                    db.SaveChanges();

                }, ref error);
            }
        }

    }
}