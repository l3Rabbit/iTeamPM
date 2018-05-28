using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
	[Table("iteam_task_lists")]
	public class iteam_task_lists //Create 9/3/2561 13:35:53
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
		public string name_task_list { get; set; }

		[MaxLength(-1)]
		public string remark_list { get; set; }

		public int? save_user { get; set; }

		public DateTime? add_dt { get; set; }

		[MaxLength(1)]
		public string active { get; set; }

		public int? edit_user { get; set; }

		public DateTime? edit_date { get; set; }

		[MaxLength(1)]
		public string status { get; set; }

	}
}