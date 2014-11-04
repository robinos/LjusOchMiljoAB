-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Views -> Hem -> Hem.cshtml som vy

Produkter har
	Controllers -> ProdukterController.cs som kodfil
	Views -> Produkter -> Index.cshtml som vy
	(Create, Delete, och Edit var automatgenererad men kommer inte att anv�ndas)

Views -> Produkter ->		
		Details.cshtml �r vyn f�r produktdetaljer
		Prislista.cshtml �r vyn f�r pristlistan

Models ->
	LOMDB_Model.edmx �r automatgenererad modellen av databasen LOM_DBEntities
	IProdukterRespository.cs �r en interface f�r implementering av kontakt med databasen
	EntityProdukterManagerRepository.cs �r implementationen av IProdukterRespository f�r
		kontakt mellan Controllers och databasen 

Anvandare �r tabellen f�r inloggningssystemet och �r inte kopplad till n�got �n.
Vi beh�ver inte anv�nda oss av den om vi hittar en annan l�sning.
	Controllers -> AnvandareController.cs �r koden
	Views -> Anvandare -> (automatgenererad Create, Delete, Details, Edit, Index.cshtml)


-OVRIGT--------------------------------------------------------------------------------
�vriga automatgenererad filer av betydelse

Som default startas ett MVC basprojekt med registrering och inloggningsformer
- Controllers ->
	- AccountController
	- ManageController
- Views -> Account ->
	- en massa vyn f�r registrering och login
	- form f�r att byta l�senord, mm

Den har �ven vyer som �r delad f�r alla vyn (meny)
- Views -> Shared ->
	- _Layout.cshtml (meny layout)
	- _LoginPartial.cshtml (som visar om man �r inloggad eller inte)
	- Error.cshtml (n�got gick snett)
	- Lockout.cshtml (banad konto)

App_Start mappen inneh�ller n�gra configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med l�nkar)
- FilterConfig.cs �r en inbyggt datafilter


-TESTNING, LjusOchMiljoAB.Tests--------------------------------------------------------
LjusOchMiljoAB.Tests bara inneh�ller n�gra automatgenererad tester plus b�rjan p�
testningen

Models ->
	InMemoryProdukterRepository.cs �r en mock repository f�r testning som implementerar
		IProdukterRepository

Controllers ->
	HemControllerTest.cs inneh�ller tester f�r huvudsidan (automatgenererad �n s� l�nge)
		TestMetoder:
			HemIndex testar 
			HemOmHarDefaultText testar om Om-sidan har texten "Om hemsidan och Ljus och
				Milj� AB"
			HemKontaktHarDefaultText testar om Kontakt-sidan har textan "Kontaktsidan"	
	ProdukterControllerTest.cs inneh�ller metoder H�mtaProdukt och ProdukterController, och
		inreklassen MockHttpContext, f�r att g�r allt redo f�r testning.
		TestMetoder:
			ProdukterIndexNotNull kollar att Index vyn vid laddning med inga parameter
				�r inte noll
			(Fler metoder kommer snart)


F�r �ndringar sedan sist, ser Changelog.txt

