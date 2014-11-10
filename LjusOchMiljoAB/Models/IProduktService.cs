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
		IEnumerable<Produkt> HämtaProdukter();
		SelectList HämtaValLista(IEnumerable<Produkt> produkter, string produktTyp);
		IEnumerable<Produkt> HämtaFiltreradProduktlista(IEnumerable<Produkt> produkter, string produktTyp, string sökSträng);
		IEnumerable<Produkt> HämtaOrdnadProduktlista(IEnumerable<Produkt> produkter, string Ordning);
		IPagedList HämtaSida(IEnumerable<Produkt> produkter, int? sida);
		Produkt HämtaProduktMedID(string id);
		void Förstör();
	}
}
