using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTeamPM.Models.DataModels
{
	[Table("iteam_comment")]
	public class iteam_comment //Create 2/3/2561 11:03:38
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int? task_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? itemno { get; set; }

		public string comment { get; set; }

		public int? add_user { get; set; }

		public int? edit_user { get; set; }

		public DateTime? add_dt { get; set; }

		public DateTime? edit_dt { get; set; }

	}
}