using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace iTeamPM.Models.Home
{
    public class Home
    {
        private Models.Auth auth;
        public Home(Models.Auth auth)
        {
            this.auth = auth;

        }

        public dynamic ReadNotifications(ref int total_project, ref int total_tasks)
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_history_project
                            join b1 in db.iteam_project on new { a.project_id } equals new { b1.project_id } into b2
                            from b in b2.DefaultIfEmpty()

                            where a.user_id == auth.user_id && a.active == "Y"

                            let username = db.iteam_user.Where(x => x.user_id == b.add_user).Select(s => s.name_th).FirstOrDefault()

                            select new
                            {
                                b.project_id,
                                b.project_name,
                                username,
                                b.add_dt,
                                a.read_notify
                            }).Where(x => x.read_notify == "N").Distinct().OrderByDescending(o => o.project_id).ToList();

                var tasks_data = (from a in db.iteam_task
                                  join b1 in db.iteam_project on new { a.project_id } equals new { b1.project_id } into b2
                                  from b in b2.DefaultIfEmpty()
                                  join c1 in db.iteam_task_user on new { a.task_id } equals new { c1.task_id } into c2
                                  from c in c2.DefaultIfEmpty()

                                  where c.user_id == auth.user_id || a.add_user == auth.user_id

                                  select new
                                  {
                                      b.project_id,
                                      b.project_name,
                                      a.task_id,
                                      a.tasks_name,
                                      a.add_dt,
                                  }).Distinct().ToList();

                var tasks_list = (from a in db.iteam_task
                                  join b1 in db.iteam_task_lists on new { a.task_id } equals new { b1.task_id } into b2
                                  from b in b2.DefaultIfEmpty()
                                  join c1 in db.iteam_task_user on new { a.task_id } equals new { c1.task_id } into c2
                                  from c in c2.DefaultIfEmpty()

                                  where c.user_id == auth.user_id || a.add_user == auth.user_id

                                  let project_name = db.iteam_project.Where(w => w.project_id == a.project_id).Select(s => s.project_name).FirstOrDefault()

                                  select new
                                  {
                                      project_name,
                                      a.task_id,
                                      a.tasks_name,
                                      b.name_task_list,
                                      b.status,
                                      b.add_dt,
                                  }).Distinct().ToList();

                total_project = data.Count();
                total_tasks = tasks_data.Count();
                var total_list = tasks_list.Count();

                output = new
                {
                    project_data = data,
                    tasks_data = tasks_data,
                    tasks_list = tasks_list,
                    total = total_project + total_tasks + total_list,
                };
            }

            return output;
        }

        public void UpdateReadProject(int? project_id, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var data = db.iteam_history_project.Where(x => x.project_id == project_id && x.user_id == auth.user_id).FirstOrDefault();
                    if (data != null)
                    {
                        data.read_notify = "Y";
                        db.SaveChanges();
                    }
                }, ref error);
            }

        }

        public dynamic ReadData()
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {
                var data = db.iteam_user.ToList();
                output = data;
            }

            return output;
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

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()
                            let count_tasks = db.iteam_task.Where(x => x.project_id == a.project_id).Count()
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
                                is_overdue = DbFunctions.DiffDays(DateTime.Now, a.end_project),
                                count_tasks,
                                username,
                                memberProject
                            }).ToList();

                output = data;

            }

            return output;
        }

        public dynamic ReadTasks()
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

                            let project_name = db.iteam_project.Where(x => x.project_id == a.project_id).Select(x => x.project_name).FirstOrDefault()

                            let username = db.iteam_user.Where(x => x.user_id == a.add_user).Select(x => x.name_th).FirstOrDefault()
                            where memberTasks.Select(x => x.user_id).Contains(auth.user_id) || a.add_user == auth.user_id

                            select new
                            {
                                a.project_id,
                                a.task_id,
                                a.tasks_name,
                                a.add_user,
                                a.add_dt,
                                a.tasks_prioity,
                                project_name,
                                username,
                                memberTasks
                            }).ToList();

                output = data;

            }

            return output;
        }

        public dynamic ViewGroup()
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

                output = data;

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

    }
}