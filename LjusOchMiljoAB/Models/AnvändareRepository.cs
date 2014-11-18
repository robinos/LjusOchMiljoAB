using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace LjusOchMiljoAB.Models
{
	/* 
	 * AnvändareRepository är implementationen av IAnvändareRepository för
	 * kontakt med database tabellen Anvandare.
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public class AnvändareRepository : IAnvändareRepository
	{
		//instansvariabler
		//Databas objektet
		private LOM_DBEntities db = new LOM_DBEntities();

		/*
		 * RedigeraAnvändare ändrar en rad i tabellen Anvandare baserad på
		 * Anvandare objektet.
		 * 
		 * in: Anvandare objektet som kommer att ändras
		 * ut: Task för await
		 */
		public async Task RedigeraAnvändare(Anvandare användareAttÄndra)
		{
			db.Entry(användareAttÄndra).State = EntityState.Modified;
			await SparaÄndringar();
		}

		/*
		 * HämtaAnvändareMedNamn hämtar data från en rad i databasen som har 
		 * användarnamnet och sätter det i en Anvandare objekt (om användarnamnet
		 * finns).
		 * 
		 * Kastar System.ArgumentNullException
		 * 
		 * in: användarnamn som rensade sträng
		 * ut: Task för await och Anvandare objekt som matchar
		 */
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
				//Exception hantering kunde vara här
				throw ex;
			}

			return användare;
		}

		/*
		 * SparaÄndringar sparar ändringar till databasen.  Vid fel kastas en
		 * exception.  SparaÄndring kör async trots att den är synchronous för
		 * att göra tasks som väntar på den async.
		 * 
		 * Kastar System.Data.Entity.Infrastructure.DbUpdateException, 
		 * System.Data.Entity.Infrastructure.DbUpdateConcurrencyException, 
		 * System.Data.Entity.Validation.DbEntityValidationException, 
		 * System.NotSupportedException, System.ObjectDisposedException,
		 * System.InvalidOperationException
		 * 
		 * ut: Task för await och int resultat från att spara till databasen
		 */
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

		/*
		 * Förstör tar bort instansen av databasen och kan användas för minnesrengöring
		 * vid applikationsslut.
		 */
		public async Task Förstör()
		{
			await Task.Delay(0);	
			db.Dispose();			
		}

		/*
		 * Bara för implementation av SkapaAnvändare fär IAnvändareTjänst
		 */
		public void SkapaAnvändare(Anvandare användareAttTillägga)
		{
			//ingenting
		}
	}
}