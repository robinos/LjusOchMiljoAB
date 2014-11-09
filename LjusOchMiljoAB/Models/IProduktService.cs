using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

/* 
 * IProduktService
 * 
 * Grupp 2
 * Senast ändrat: 2014 11 09
 * Version: 0.17
 */
namespace LjusOchMiljoAB.Models
{
	public interface IProduktService
	{
		IEnumerable<Produkter> HämtaProdukter();
		SelectList HämtaValLista(IEnumerable<Produkter> produkter, string produktTyp);
		IEnumerable<Produkter> HämtaFiltreradProduktlista(IEnumerable<Produkter> produkter, string produktTyp, string sökSträng);
		IEnumerable<Produkter> HämtaOrdnadProduktlista(IEnumerable<Produkter> produkter, string Ordning);
		IPagedList HämtaSida(IEnumerable<Produkter> produkter, int? sida);
		Produkter HämtaProduktMedID(string id);
		void Förstör();
	}
}
