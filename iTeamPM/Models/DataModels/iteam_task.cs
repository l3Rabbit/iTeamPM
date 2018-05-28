using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
	[Table("iteam_task")]
	public class iteam_task //Create 28/2/2561 8:44:27
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? task_id { get; set; }

		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int? project_id { get; set; }

		[MaxLength(-1)]
		public string tasks_name { get; set; }

		[MaxLength(-1)]
		public string tasks_description { get; set; }

		public DateTime? date_start { get; set; }

		public DateTime? date_end { get; set; }

		[MaxLength(-1)]
		public string tasks_prioity { get; set; }

		public DateTime? add_dt { get; set; }

		[MaxLength(-1)]
		public string path_file { get; set; }

		public int? add_user { get; set; }

        [MaxLength(1)]
        public string status { get; set; }

    }
}