using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_project")]
    public class iteam_project //Create 30/4/2561 8:30:35
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? project_id { get; set; }

        [MaxLength(-1)]
        public string project_name { get; set; }

        [MaxLength(-1)]
        public string project_des { get; set; }

        public DateTime? start_project { get; set; }

        public DateTime? end_project { get; set; }

        public DateTime? add_dt { get; set; }

        public int? add_user { get; set; }

        public DateTime? edit_dt { get; set; }

        public int? tasks_complete { get; set; }

        public int? tasks_count { get; set; }

        [MaxLength(1)]
        public string status_project { get; set; }

    }
}