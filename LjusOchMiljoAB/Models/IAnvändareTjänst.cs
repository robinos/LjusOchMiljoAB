using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace LjusOchMiljoAB.Models
{
	/*
	 * IAnvändareTjänst är interface för inloggningssystemet.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public interface IAnvändareTjänst
	{
		Task<Status> BekräftaLösenord(string användarnamn, string lösenord);
		Task Förstör();
	}

	//Status enum används med BekräftaLösenord och är definerad i denna filen
	//för att det har med inloggningssystemet att göra.
	public enum Status { Misslyckades, Lyckades, Låste };
}
