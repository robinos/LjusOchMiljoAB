using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

/// <summary>
/// IProduktTjänst är interface för produktlistan. 
/// 
/// Version: 1.0
/// 2014-12-14
/// Grupp 2
/// </summary>
namespace LjusOchMiljoAB.Models
{
	public interface IProduktTjänst
	{
		Task<IEnumerable<Produkt>> HämtaProdukter();
		SelectList HämtaValLista(IEnumerable<Produkt> produkter, string produktTyp);
		IEnumerable<Produkt> HämtaFiltreradProduktlista(IEnumerable<Produkt> produkter, string produktTyp, string sökSträng);
		IEnumerable<Produkt> HämtaOrdnadProduktlista(IEnumerable<Produkt> produkter, string Ordning);
		IPagedList HämtaSida(IEnumerable<Produkt> produkter, int? sida);
		Task<Produkt> HämtaProduktMedID(string id);
		Task Förstör();
	}
}
