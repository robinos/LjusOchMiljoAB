using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace LjusOchMiljoAB.Models
{
	/// <summary>
	/// IAnvändareTjänst är en interface för inloggningssystemet.
	/// 
	/// Version: 1.0
	/// 2014-12-10
	/// Grupp 2
	/// </summary>
	public interface IAnvändareTjänst
	{
		Task<Status> BekräftaLösenord(string användarnamn, string lösenord);
		Task Förstör();
		void Inloggning(string username);
		void Utloggning();
		void SkapaAnvändare(Anvandare användareAttTillägga);
	}

	/// <summary>
	/// Status enum används med BekräftaLösenord och är definerad i denna filen
	/// för att det har med inloggningssystemet att göra.
	/// </summary>
	public enum Status { Misslyckades, Lyckades, Låst };
}
