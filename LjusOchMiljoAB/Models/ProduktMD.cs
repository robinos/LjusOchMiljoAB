using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LjusOchMiljoAB.Models
{
	[MetadataType(typeof(ContactMD))]
	public partial class Contact
	{
		public class ContactMD
		{
			[Required()]
			public object ID { get; set; }
			[Required()]
			public object Namn { get; set; }
			public object Pris { get; set; }
			public object Typ { get; set; }
			public object Farg { get; set; }
			public object Bildfilnamn { get; set; }
			public object Ritningsfilnamn { get; set; }
			public object RefID { get; set; }
			public object Beskrivning { get; set; }
			public object Montering { get; set; }
		}
	}
}