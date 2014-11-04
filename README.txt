-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Views -> Hem -> Hem.cshtml som vy

Produkter har
	Controllers -> ProdukterController.cs som kodfil
	Views -> Produkter -> Index.cshtml som vy
	(Create, Delete, och Edit var automatgenererad men kommer inte att användas)

Views -> Produkter ->		
		Details.cshtml är vyn för produktdetaljer
		Prislista.cshtml är vyn för pristlistan

Models ->
	LOMDB_Model.edmx är automatgenererad modellen av databasen LOM_DBEntities
	IProdukterRespository.cs är en interface för implementering av kontakt med databasen
	EntityProdukterManagerRepository.cs är implementationen av IProdukterRespository för
		kontakt mellan Controllers och databasen 

Anvandare är tabellen för inloggningssystemet och är inte kopplad till något än.
Vi behöver inte använda oss av den om vi hittar en annan lösning.
	Controllers -> AnvandareController.cs är koden
	Views -> Anvandare -> (automatgenererad Create, Delete, Details, Edit, Index.cshtml)


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
	InMemoryProdukterRepository.cs är en mock repository för testning som implementerar
		IProdukterRepository

Controllers ->
	HemControllerTest.cs innehåller tester för huvudsidan (automatgenererad än så länge)
		TestMetoder:
			HemIndex testar 
			HemOmHarDefaultText testar om Om-sidan har texten "Om hemsidan och Ljus och
				Miljö AB"
			HemKontaktHarDefaultText testar om Kontakt-sidan har textan "Kontaktsidan"	
	ProdukterControllerTest.cs innehåller metoder HämtaProdukt och ProdukterController, och
		inreklassen MockHttpContext, för att gör allt redo för testning.
		TestMetoder:
			ProdukterIndexNotNull kollar att Index vyn vid laddning med inga parameter
				är inte noll
			(Fler metoder kommer snart)


För ändringar sedan sist, ser Changelog.txt

