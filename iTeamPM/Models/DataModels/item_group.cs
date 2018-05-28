using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{

	[Table("iteam_group")]
	public class iteam_group //Create 19/2/2561 16:52:35
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? group_id { get; set; }

		[MaxLength(-1)]
		public string group_name { get; set; }

		[MaxLength(-1)]
		public string group_description { get; set; }

		public int? add_user { get; set; }

		public DateTime? add_dt { get; set; }

		public int? edit_user { get; set; }

		public DateTime? edit_dt { get; set; }

	}

}