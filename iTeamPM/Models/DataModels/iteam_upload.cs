using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
	[Table("iteam_upload")]
	public class iteam_upload //Create 11/4/2561 8:31:32
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int? task_id { get; set; }

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