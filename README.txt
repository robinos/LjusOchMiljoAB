-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Controllers -> AnvändareTjänst.cs som användarehanteringstjänst
	Views -> Hem -> Index.cshtml som vy
	Views -> Inloggning -> Inloggning.cshtml som inloggning

Produkter har
	Controllers -> ProduktController.cs som kodfil
	Controllers -> ProduktTjänst.cs som produkthanteringstjänst
	Views -> Produkt -> Index.cshtml som vy

Annars:
Views -> Produkter ->		
		Details.cshtml är vyn för produktdetaljer
		Prislista.cshtml är vyn för pristlistan
Views -> Hem ->	
		SkapaAnvändare.cshtml är bara för att skapa test användare

Models ->huhu
	LOMDB_Model.edmx är automatgenererad modellen av databasen LOM_DBEntities
	IProduktRespository.cs är en interface för implementering av kontakt med produkt
		tabellen
	ProduktRepository.cs är implementationen av IProdukterRespository för
		kontakt mellan ProduktTjänst och databasen 
	IAnvändareRespository.cs är en interface för implementering av kontakt med användare
		tabellen
	AnvändareRepository.cs är implementationen av IAnvändareRespository för
		kontakt mellan AnvändareTjänst och databasen 


-OVRIGT--------------------------------------------------------------------------------
Övriga automatgenererad filer av betydelse

Som default startas ett MVC basprojekt med registrering och inloggningsformer
- Controllers ->
	- AccountController
	- ManageController
- Views -> Account ->
	- en massa vyn för registrering och login
	- form för att byta lösenord, mm

Den har även vyer som är delad för alla vyn (meny)
- Views -> Shared ->
	- _Layout.cshtml (meny layout)
	- _LoginPartial.cshtml (som visar om man är inloggad eller inte)
	- Error.cshtml (något gick snett)
	- Lockout.cshtml (banad konto)

App_Start mappen innehåller några configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med länkar)
- FilterConfig.cs är en inbyggt datafilter


-TESTNING, LjusOchMiljoAB.Tests--------------------------------------------------------
LjusOchMiljoAB.Tests bara innehåller några automatgenererad tester plus början på
testningen

Models ->
	IMinnetProdukterRepository.cs är en mock repository för testning som implementerar
		IProdukterRepository

Controllers ->
	HemControllerTest.cs innehåller testar för huvudsidan (automatgenererad än så länge)

	ProduktControllerTest.cs innehåller metoder HämtaProdukt och ProduktController, och
		inreklassen MockHttpContext, för att göra allt redo för testning.  Den innehåller
		testar för att rätt vyn visas för produktlistan och produkten.

	ProduktTjänstTest.cs innehåller metoder HämtaProdukt och ProduktTjänst för att göra
		allt redo för testning.  Den innehåller testar för att rätt lista av produkter
		returneras. 

För ändringar sedan sist ser Changelog.txt

