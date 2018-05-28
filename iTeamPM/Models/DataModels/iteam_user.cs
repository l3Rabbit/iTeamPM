using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_user")]
    public class iteam_user //Create 30/4/2561 8:31:01
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? user_id { get; set; }

        [MaxLength(100)]
        public string username { get; set; }

        [MaxLength(100)]
        public string password { get; set; }

        [MaxLength(-1)]
        public string name_th { get; set; }

        [MaxLength(-1)]
        public string postion { get; set; }

        public string path_image { get; set; }

        [MaxLength(-1)]
        public string description { get; set; }

        [MaxLength(10)]
        public string phone { get; set; }

        [MaxLength(50)]
        public string line_id { get; set; }

        [MaxLength(200)]
        public string email { get; set; }

        [MaxLength(1)]
        public string is_admin { get; set; }

    }
}