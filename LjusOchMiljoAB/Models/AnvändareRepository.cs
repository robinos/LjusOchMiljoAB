using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LjusOchMiljoAB.Models
{
	/* 
	 * AnvändareRepository x
	 * 
	 * Grupp 2
	 * Senast ändrat: 2014 11 11
	 * Version: 0.18
	 */
	public class AnvändareRepository : IAnvändareRepository
	{
		//instansvariabler
		//Databas objekten
		private LOM_DBEntities db = new LOM_DBEntities();

		public void SkapaAnvändare(Anvandare användareAttSkapa)
		{
			db.Anvandare.Add(användareAttSkapa);
			db.SaveChanges();
		}
		public void TaBortAnvändare(string användarnamn)
		{
			//var conToDel = HämtaAnvändareMedNamn(användarnamn);
			//db.Anvandare.Remove(conToDel);
			//db.SaveChanges();
		}
		public void RedigeraAnvändare(Anvandare användareAttÄndra)
		{
			db.Entry(användareAttÄndra).State = EntityState.Modified;
			db.SaveChanges();
		}
		public Anvandare HämtaAnvändareMedNamn(string användarnamn)
		{
			return db.Anvandare.FirstOrDefault(d => d.Anvandarnamn == användarnamn);
		}
		public int SparaÄndringar()
		{
			return db.SaveChanges();
			//return 1;
		}
		public void Förstör()
		{
			db.Dispose();			
		}
	}
}