using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_upload_pic")]
    public class iteam_upload_pic //Create 4/25/2018 2:59:50 PM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? news_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? itemno { get; set; }

        [MaxLength(-1)]
        public string path_file { get; set; }

        [MaxLength(-1)]
        public string file_name { get; set; }

        public int? add_user { get; set; }

        public DateTime? add_dt { get; set; }

    }
}