using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/// <summary>
	/// AnvändareRepository är implementationen av IAnvändareRepository för
	/// kontakt med database tabellen Anvandare.
	/// 
	/// -Metoder-
	/// RedigeraAnvändare - ändrar en rad i tabellen Anvandare baserad på en Anvandare
	///		objekt med samma id
	/// HämtaAnvändareMedNamn - hämtar data från en rad i databasen som har
	///		användarnamnet och spara det till en Anvandare objekt som skickas tillbaka
	/// SparaÄndringar - sparar ändringar till databasen
	/// Förstör - för minnesrengöring vid applikationsslut
	/// SkapaAnvändare - tom metod
	/// 
	/// Version: 1.0
	/// 2014-12-12
	/// Grupp 2
	/// </summary>
	public class AnvändareRepository : IAnvändareRepository
	{
		//instansvariabler
		//Databas objektet
		private LOM_DBEntities db = new LOM_DBEntities();

		/// <summary>
		/// RedigeraAnvändare ändrar en rad i tabellen Anvandare baserad på
		/// Anvandare objektet.
		/// </summary>
		/// <param name="användareAttÄndra">Anvandare objektet att ändra</param>
		/// <returns>Task för async (ingen data returneras)</returns>
		public async Task RedigeraAnvändare(Anvandare användareAttÄndra)
		{
			db.Entry(användareAttÄndra).State = EntityState.Modified;
			await SparaÄndringar();
		}

		/// <summary>
		/// HämtaAnvändareMedNamn hämtar data från en rad i databasen som har 
		/// användarnamnet och sätter det i en Anvandare objekt (om användarnamnet
		/// finns).
		/// 
		/// Kastar System.ArgumentNullException
		/// </summary>
		/// <param name="användarnamn">Sträng användarnamn att söka efter i tabellen</param>
		/// <returns>Task för async och Anvandare objektet som hittades eller default
		///		(eller null vid exception)</returns>
		public async Task<Anvandare> HämtaAnvändareMedNamn(string användarnamn)
		{
			await Task.Delay(0);
			
			Anvandare användare = null;

			try
			{
				användare = db.Anvandare.FirstOrDefault(d => d.Anvandarnamn == användarnamn);
			}
			catch(Exception ex)
			{
				//Exception loggning kunde vara här
			}

			return användare;
		}

		/// <summary>
		/// SparaÄndringar sparar ändringar till databasen.  Vid fel kastas en
		/// exception.  SparaÄndring kör async trots att den är synchronous för
		/// att göra tasks som väntar på den async.
		/// 
		/// Kastar System.Data.Entity.Infrastructure.DbUpdateException,
		/// System.Data.Entity.Infrastructure.DbUpdateConcurrencyException,
		/// System.Data.Entity.Validation.DbEntityValidationException,
		/// System.NotSupportedException, System.ObjectDisposedException,
		/// System.InvalidOperationException
		/// </summary>
		/// <returns>Task för async och int resultat av att spara till databasen</returns>
		public async Task<int> SparaÄndringar()
		{
			await Task.Delay(0);

			int värde = 0;

			try
			{
				värde = db.SaveChanges();
			}
			catch(Exception ex)
			{
				//Exception hantering kunde vara här
				throw ex;
			}

			return värde;
		}

		/// <summary>
		/// Förstör tar bort instansen av databasen och kan användas för
		/// minnesrengöring vid applikationsslut.
		/// </summary>
		/// <returns>Task för async (ingen data returneras)</returns>
		public async Task Förstör()
		{
			await Task.Delay(0);	
			db.Dispose();			
		}

		/// <summary>
		/// Bara för implementation av SkapaAnvändare för IAnvändareTjänst.
		/// </summary>
		/// <param name="användareAttTillägga">Anvandare objektet att lägga till</param>
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			//ingenting
		}
	}
}