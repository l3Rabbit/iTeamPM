using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTeamPM.Models.DataModels;
using iTeamPM.Models.Database;
using System.Data.Entity.Validation;
namespace iTeamPM.Models.Contact
{
    public class Contact
    {
        private Models.Auth auth;
        public Contact(Models.Auth auth)
        {
            this.auth = auth;

        }
        public void ContactAdd(DataModels.iteam_contact data, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var user_name = data?.user_name;
                    var des = data?.des;
                    var phone = data?.phone;
                    var email = data?.email;



                    if (string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(des) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email))
                    {
                        throw new Exception("โปรดกรอกข้อความก่อนส่ง");
                    }

                    data.add_date = DateTime.Now;
                    data.add_user = auth.user_id;
                    data.status = "N";
                    data.read = "N";
                    db.iteam_contact.Add(data);
                    db.SaveChanges();

                }, ref error);
            }
        }

        public dynamic ReadContact(string text, int skip, int take, ref int total)
        {
            dynamic output = new { };
            using (var db = new DataContext())
            {
                var data = db.iteam_contact.Where(z => 1 == 1);

                var text_arr = (!string.IsNullOrEmpty(text)) ? text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x)).Take(3).ToList() : new List<string> { };
                if (text_arr.Count > 0)
                {
                    text_arr.ForEach(x => data = data.Where(z => z.user_name.Contains(x)));
                }

                total = data.Count();
                var dataTemplate = data.ToList();
                var final = dataTemplate.Skip(skip).Take(take).ToList();
                output = final;
            }
            return output;
            //dynamic output = new { };

            //using (var db = new DataContext())
            //{
            //    var data = db.iteam_contact.ToList();

            //    output = data;

            //}

            //return output;
        }

        public dynamic ContactReadDetail(int? contact_id)
        {
            dynamic output = new { };

            using (var db = new DataContext())
            {
                var data = (from a in db.iteam_contact

                            where a.contact_id == contact_id

                            select new
                            {
                                a.contact_id,
                                a.user_name,
                                a.phone,
                                a.email,
                                a.des,
                                a.add_user,
                                a.add_date,
                                a.status,
                                a.read


                            }).FirstOrDefault();

                output = data;

            }

            return output;
        }

        public void ChangeRead(iteam_contact m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var contact_id = m?.contact_id;
            

                    var data = db.iteam_contact.Where(x => x.contact_id == contact_id).FirstOrDefault();

                    
                       
                        data.read = "Y";
                       db.SaveChanges();
                    

                }, ref error);
            }
        }

        public void ChangeStatus(iteam_contact m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var contact_id = m?.contact_id;


                    var data = db.iteam_contact.Where(x => x.contact_id == contact_id).FirstOrDefault();



                    data.status = "Y";
                    db.SaveChanges();


                }, ref error);
            }
        }

        public void ContactDelete(Models.DataModels.iteam_contact m, ref string error)
        {
            using (var db = new DataContext())
            {
                db.ExecuteTransaction(() =>
                {
                    var contact_id = m?.contact_id;
                  

                    db.iteam_contact.RemoveRange(db.iteam_contact.Where(w => w.contact_id == contact_id));
                    db.SaveChanges();

                }, ref error);
            }
        }

    }
}