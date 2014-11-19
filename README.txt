-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Controllers -> AnvändareTjänst.cs som användarehanteringstjänst
	Views -> Hem -> Index.cshtml som vy
	Views -> Hem -> Inloggning -> Inloggning.cshtml som inloggning
	Views -> Hem -> Utlåste -> Utlåste.cshtml som vy vid utlåsning
	Views -> Hem -> Om -> Om.cshtml är Om-sidan
	Views -> Hem -> Kontakt -> Kontakt.cshtml är Kontaktsidan

Produkter har
	Controllers -> ProduktController.cs som kodfil
	Controllers -> ProduktTjänst.cs som produkthanteringstjänst
	Views -> Produkt -> Index.cshtml som vy
	Views -> Produkt -> Details.cshtml är vyn för produktdetaljer
	Views -> Produkt -> Prislista.cshtml är vyn för pristlistan

Views -> Hem ->	
		SkapaAnvändare.cshtml är bara för att skapa test användare och används inte
Views -> Shared -> _Layout.cshtml (meny layout)


Models ->
	IProduktRespository.cs är en interface för implementering av kontakt med produkt
		tabellen
	ProduktRepository.cs är implementationen av IProdukterRespository för
		kontakt mellan ProduktTjänst och databasen 
	IProduktTjänst.cs är en interface för implementering av kontakt mellan en
		IProduktRepository och ProduktController
	IAnvändareRespository.cs är en interface för implementering av kontakt med användare
		tabellen
	AnvändareRepository.cs är implementationen av IAnvändareRespository för
		kontakt mellan AnvändareTjänst och databasen
	IAnvändareTjänst.cs är en interface för implementering av kontakt mellan en
		IAnvändareRepository och HemController, och för inloggningssystemet		 
	InloggningsModell.cs är modellen för inloggningsformen och används för
		verifikation.
	LOMDB_Model.edmx är automatgenererad modellen av databasen LOM_DBEntities
		-> LOMDB_Model.tt har datan för modellen
			-> Anvandare.cs är modellen för Anvandare tabellen som c# fil
			-> LOMDB_Model.cs är en tom fil, antagligen partial
			-> Produkt.cs är modellen för Produkt tabellen som c# fil


-OVRIGT--------------------------------------------------------------------------------
Övriga filer av betydelse

App_Start mappen innehåller några configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med länkar)
- FilterConfig.cs är en inbyggt datafilter

App_Data innehåller databasen som LOM_DBEntities är baserad på

Alla automatgenererad filer för inloggning inklusive AccountController och
ManageController används INTE.  HemController och AnvändareTjänst används
istället.  Även det mesta under Views -> Shared används inte förutom
	- _Layout.cshtml (meny layout)
	- Error.cshtml (något gick snett)


-TESTNING, LjusOchMiljoAB.Tests--------------------------------------------------------
LjusOchMiljoAB.Tests bara innehåller några automatgenererad tester plus början på
testningen

Models ->
	IMinnetProduktRepository.cs är en mock repository för testning som implementerar
		IProduktRepository
	IMinnetAnvändareRepository.cs är en mock repository för testning som implementerar
		IAnvändareRepository

Controllers ->
	HemControllerTest.cs innehåller testar för huvudsidan, omsidan, kontaktsidan och
		inloggning/utloggning.  Den använder MvcContrib.TestHelper för automocking som
		ersätter MockHttpContext och kan förhoppningsvis användas sedan för att även
		ersätter Mock respositories och tjänster så man behöver inte lika mycket kod. 

	IMinnetAnvändareTjänst.cs innehåller en mock tjänst för testning som implementerar
		IAnvändareTjänst

	AnvändareTjänstTest.cs innehåller metoder HämtaAnvändare och AnvändareRepository för
		att göra allt redo för testning.  Den innehåller testar för att bekräfta
		lösenordet. 

	ProduktControllerTest.cs innehåller metoder HämtaProdukt och ProduktController, och
		inreklassen MockHttpContext, för att göra allt redo för testning.  Den innehåller
		testar för att rätt vyn visas för produktlistan och produkten.

	ProduktTjänstTest.cs innehåller metoder HämtaProdukt och ProduktTjänst för att göra
		allt redo för testning.  Den innehåller testar för att rätt lista av produkter
		returneras. 

För ändringar sedan sist ser Changelog.txt

