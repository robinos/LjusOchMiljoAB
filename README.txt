-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Controllers -> Anv�ndareTj�nst.cs som anv�ndarehanteringstj�nst
	Views -> Hem -> Index.cshtml som vy
	Views -> Hem -> Inloggning -> Inloggning.cshtml som inloggning
	Views -> Hem -> Utl�ste -> Utl�ste.cshtml som vy vid utl�sning
	Views -> Hem -> Om -> Om.cshtml �r Om-sidan
	Views -> Hem -> Kontakt -> Kontakt.cshtml �r Kontaktsidan

Produkter har
	Controllers -> ProduktController.cs som kodfil
	Controllers -> ProduktTj�nst.cs som produkthanteringstj�nst
	Views -> Produkt -> Index.cshtml som vy
	Views -> Produkt -> Details.cshtml �r vyn f�r produktdetaljer
	Views -> Produkt -> Prislista.cshtml �r vyn f�r pristlistan

Views -> Hem ->	
		SkapaAnv�ndare.cshtml �r bara f�r att skapa test anv�ndare och anv�nds inte
Views -> Shared -> _Layout.cshtml (meny layout)


Models ->
	IProduktRespository.cs �r en interface f�r implementering av kontakt med produkt
		tabellen
	ProduktRepository.cs �r implementationen av IProdukterRespository f�r
		kontakt mellan ProduktTj�nst och databasen 
	IProduktTj�nst.cs �r en interface f�r implementering av kontakt mellan en
		IProduktRepository och ProduktController
	IAnv�ndareRespository.cs �r en interface f�r implementering av kontakt med anv�ndare
		tabellen
	Anv�ndareRepository.cs �r implementationen av IAnv�ndareRespository f�r
		kontakt mellan Anv�ndareTj�nst och databasen
	IAnv�ndareTj�nst.cs �r en interface f�r implementering av kontakt mellan en
		IAnv�ndareRepository och HemController, och f�r inloggningssystemet		 
	InloggningsModell.cs �r modellen f�r inloggningsformen och anv�nds f�r
		verifikation.
	LOMDB_Model.edmx �r automatgenererad modellen av databasen LOM_DBEntities
		-> LOMDB_Model.tt har datan f�r modellen
			-> Anvandare.cs �r modellen f�r Anvandare tabellen som c# fil
			-> LOMDB_Model.cs �r en tom fil, antagligen partial
			-> Produkt.cs �r modellen f�r Produkt tabellen som c# fil


-OVRIGT--------------------------------------------------------------------------------
�vriga filer av betydelse

App_Start mappen inneh�ller n�gra configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med l�nkar)
- FilterConfig.cs �r en inbyggt datafilter

App_Data inneh�ller databasen som LOM_DBEntities �r baserad p�

Alla automatgenererad filer f�r inloggning inklusive AccountController och
ManageController anv�nds INTE.  HemController och Anv�ndareTj�nst anv�nds
ist�llet.  �ven det mesta under Views -> Shared anv�nds inte f�rutom
	- _Layout.cshtml (meny layout)
	- Error.cshtml (n�got gick snett)


-TESTNING, LjusOchMiljoAB.Tests--------------------------------------------------------
LjusOchMiljoAB.Tests bara inneh�ller n�gra automatgenererad tester plus b�rjan p�
testningen

Models ->
	IMinnetProduktRepository.cs �r en mock repository f�r testning som implementerar
		IProduktRepository
	IMinnetAnv�ndareRepository.cs �r en mock repository f�r testning som implementerar
		IAnv�ndareRepository

Controllers ->
	HemControllerTest.cs inneh�ller testar f�r huvudsidan, omsidan, kontaktsidan och
		inloggning/utloggning.  Den anv�nder MvcContrib.TestHelper f�r automocking som
		ers�tter MockHttpContext och kan f�rhoppningsvis anv�ndas sedan f�r att �ven
		ers�tter Mock respositories och tj�nster s� man beh�ver inte lika mycket kod. 

	IMinnetAnv�ndareTj�nst.cs inneh�ller en mock tj�nst f�r testning som implementerar
		IAnv�ndareTj�nst

	Anv�ndareTj�nstTest.cs inneh�ller metoder H�mtaAnv�ndare och Anv�ndareRepository f�r
		att g�ra allt redo f�r testning.  Den inneh�ller testar f�r att bekr�fta
		l�senordet. 

	ProduktControllerTest.cs inneh�ller metoder H�mtaProdukt och ProduktController, och
		inreklassen MockHttpContext, f�r att g�ra allt redo f�r testning.  Den inneh�ller
		testar f�r att r�tt vyn visas f�r produktlistan och produkten.

	ProduktTj�nstTest.cs inneh�ller metoder H�mtaProdukt och ProduktTj�nst f�r att g�ra
		allt redo f�r testning.  Den inneh�ller testar f�r att r�tt lista av produkter
		returneras. 

F�r �ndringar sedan sist ser Changelog.txt

