using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_history_project")]
    public class iteam_history_project //Create 5/5/2561 15:15:30
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? project_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? user_id { get; set; }

        [MaxLength(1)]
        public string read_notify { get; set; }

        public int? add_user { get; set; }

        [MaxLength(1)]
        public string active { get; set; }

        [MaxLength(-1)]
        public string project_name { get; set; }

    }
}