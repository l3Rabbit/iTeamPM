using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;

namespace iTeamPM.Models.Account
{
    public class Account
    {
        public void Login(string username, string password, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
                    {
                        throw new Exception("โปรดกรอกรหัสผู้ใช้งาน !");
                    }

                    var user = (from a in db.iteam_user
                                where a.username == username
                                select a).FirstOrDefault();

                    if (user == null)
                    {
                        throw new Exception("ไม่มีชื่อผู้ใช้นี้ !");
                    }

                    if (user.password != password)
                    {
                        throw new Exception("รหัสผ่านผิดพลาด !");
                    }

                    HttpContext.Current.Session["Login"] = "1";

                }, ref error);

                HttpContext.Current.Session["Login"] = "";

            }
        }

        public void Register(DataModels.iteam_user m, ref string error)
        {
            using (var db = new DataContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var username = m?.username;
                        var password = m?.password.Trim();
						var name_th = m?.name_th;
                        var email = m?.email.Trim();

                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name_th))
                        {
                            throw new Exception("Error : โปรดกรอกข้อมูลให้ครบถ้วน");
                        }

                        var data = db.iteam_user.Where(x => x.username == username).FirstOrDefault();
                        if (data != null)
                        {
                            throw new Exception("Error : ชื่อผู้ใช้งานซ้ำกัน");
                        }

                        m.username = username;
                        m.password = password;
						m.name_th = name_th;
                        m.postion = "6";
						m.path_image = "https://westdulwichosteopaths.com/wp-content/uploads/2017/05/yonetici-icon-300x300.png";
                        m.email = email;
                        m.is_admin = "N";

                        db.iteam_user.Add(m);
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