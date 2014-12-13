using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/// <summary>
	/// IAnvändareRepository är interface för kontakt med databasen eller en
	/// mock databas vid testning.
	/// 
	/// SkapaAnvändare är bara för tester.
	/// 
	/// Version: 1.0
	/// 2014-12-12
	/// Grupp 2
	/// </summary>
	public interface IAnvändareRepository
	{
		Task RedigeraAnvändare(Anvandare användareAttÄndra);
		Task<Anvandare> HämtaAnvändareMedNamn(string användarnamn);
		Task<int> SparaÄndringar();
		Task Förstör();
		void SkapaAnvändare(Anvandare användareAttTillägga);
	}
}
