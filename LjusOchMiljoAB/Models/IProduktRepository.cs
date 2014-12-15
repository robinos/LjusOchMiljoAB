using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

/// <summary>
/// IProduktRepository är interface för kontakt med databasen eller en
/// mock databas vid testning.
/// 
/// Version: 1.0
/// 2014-12-14
/// Grupp 2
/// </summary>
namespace LjusOchMiljoAB.Models
{
	public interface IProduktRepository
	{
		Task<Produkt> HämtaProduktMedID(string id);
		Task<IEnumerable<Produkt>> HämtaProduktlista();
		Task Förstör();
	}
}