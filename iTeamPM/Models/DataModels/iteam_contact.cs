using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_contact")]
    public class iteam_contact //Create 4/24/2018 5:21:24 PM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? contact_id { get; set; }

        [MaxLength(-1)]
        public string user_name { get; set; }

        [MaxLength(10)]
        public string phone { get; set; }

        [MaxLength(50)]
        public string email { get; set; }

        [MaxLength(-1)]
        public string des { get; set; }

        public int? add_user { get; set; }

        public DateTime? add_date { get; set; }

        [MaxLength(10)]
        public string status { get; set; }

        [MaxLength(10)]
        public string read { get; set; }
    }
}