using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/*
	 * IAnvändareRepository x
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public interface IAnvändareRepository
	{
		void SkapaAnvändare(Anvandare användareAttSkapa);
		void TaBortAnvändare(string användarnamn);
		void RedigeraAnvändare(Anvandare användareAttÄndra);
		Anvandare HämtaAnvändareMedNamn(string användarnamn);
		int SparaÄndringar();
		void Förstör();
	}
}
