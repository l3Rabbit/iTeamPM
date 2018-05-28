using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
    [Table("iteam_news")]
    public class iteam_news //Create 4/23/2018 5:21:18 PM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? news_id { get; set; }

        [MaxLength(-1)]
        public string news_name { get; set; }

        [MaxLength(-1)]
        public string news_des { get; set; }

        public DateTime? add_date { get; set; }

        public int? add_user { get; set; }

        [MaxLength(20)]
        public string color { get; set; }

    }
}