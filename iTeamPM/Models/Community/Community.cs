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

namespace iTeamPM.Models.Community
{
    public class Community
    {
        private Models.Auth auth;
        public Community(Models.Auth auth)
        {
            this.auth = auth;

        }

        public void CreateNews(DataModels.iteam_news data, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var news_name = data?.news_name;
                    var news_des = data?.news_des;

                    if (string.IsNullOrEmpty(news_name) && string.IsNullOrEmpty(news_des))
                    {
                        throw new Exception("โปรดกรอกข้อความก่อนส่ง");
                    }

                    data.add_date = DateTime.Now;
                    data.add_user = auth.user_id;
                    db.iteam_news.Add(data);
                    db.SaveChanges();

                }, ref error);
            }
        }

        public void DeleteNews(DataModels.iteam_news m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {

                var news_id = m?.news_id;

                db.iteam_news.RemoveRange(db.iteam_news.Where(x => x.news_id == news_id).ToList());
                db.iteam_upload_pic.RemoveRange(db.iteam_upload_pic.Where(x => x.news_id == news_id)).ToList();

                    db.SaveChanges();

                }, ref error);
            }
        }


        public dynamic ReadNews()
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = db.iteam_news.ToList();

                output = data;

            }

            return output;
        }

        public dynamic ReadNewsDetail(int? news_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_news
							join b1 in db.iteam_upload_pic on new {a.news_id} equals new {b1.news_id} into b2
							from b in b2.DefaultIfEmpty()

                            where a.news_id == news_id

                            select new
                            {
                                a.news_id,
                                a.news_name,
                                a.news_des,
                                a.add_date,
                                a.add_user,
                                a.color,
								b.path_file
                            }).FirstOrDefault();

                output = data;

            }

            return output;
        }
    }
}