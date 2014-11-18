using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/*
	 * IAnvändareRepository är interface för kontakt med databasen eller en
	 * mock databas vid testning.
	 * 
	 * SkapaAnvändare är bara för tester.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public interface IAnvändareRepository
	{
		Task RedigeraAnvändare(Anvandare användareAttÄndra);
		Task<Anvandare> HämtaAnvändareMedNamn(string användarnamn);
		Task<int> SparaÄndringar();
		Task Förstör();
		void SkapaAnvändare(Anvandare användareAttTillägga);
	}
}
