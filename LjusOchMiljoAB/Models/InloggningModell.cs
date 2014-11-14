using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LjusOchMiljoAB.Models
{
	/*
	 * InloggningsModell används för validering av inloggningsformen.
	 */
	public class InloggningsModell
	{
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Användarnamn")]
		public string Anvandarnamn { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Lösenord")]
		public string Losenord { get; set; }
	}
}