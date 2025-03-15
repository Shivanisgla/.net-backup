using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruYum_APP.Models
{
	public class UserLogin
	{
		[Required(ErrorMessage ="UserId is Required")]
		[Display(Name ="User Id")]
		public string UID { get; set; }

		[Required(ErrorMessage ="password is Required")]
		[Display(Name ="Password")]
		[DataType(DataType.Password)]
		public string pwd { get; set; }
	}
}