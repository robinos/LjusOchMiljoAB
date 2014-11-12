using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace LjusOchMiljoAB.Models
{
	/*
	 * IAnvändareTjänst x
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public interface IAnvändareTjänst
	{
		Anvandare HämtaAnvändareMedNamn(string användarnamn);
		Status BekräftaLösenord(Anvandare anvandare, string lösenord);
		void SättLösenord(Anvandare anvandare, string lösenord);
		void SkapaAnvändare(Anvandare anvandare);
		void Förstör();
	}

	public enum Status { Misslyckades, Lyckades, Låste };
}
