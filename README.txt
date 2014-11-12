-APPLIKATION, LjusOchMiljoAB-----------------------------------------------------------
Huvudsidan har
	Controllers -> HemController.cs som kodfil
	Controllers -> Anv�ndareTj�nst.cs som anv�ndarehanteringstj�nst
	Views -> Hem -> Index.cshtml som vy
	Views -> Inloggning -> Inloggning.cshtml som inloggning

Produkter har
	Controllers -> ProduktController.cs som kodfil
	Controllers -> ProduktTj�nst.cs som produkthanteringstj�nst
	Views -> Produkt -> Index.cshtml som vy

Annars:
Views -> Produkter ->		
		Details.cshtml �r vyn f�r produktdetaljer
		Prislista.cshtml �r vyn f�r pristlistan
Views -> Hem ->	
		SkapaAnv�ndare.cshtml �r bara f�r att skapa test anv�ndare

Models ->huhu
	LOMDB_Model.edmx �r automatgenererad modellen av databasen LOM_DBEntities
	IProduktRespository.cs �r en interface f�r implementering av kontakt med produkt
		tabellen
	ProduktRepository.cs �r implementationen av IProdukterRespository f�r
		kontakt mellan ProduktTj�nst och databasen 
	IAnv�ndareRespository.cs �r en interface f�r implementering av kontakt med anv�ndare
		tabellen
	Anv�ndareRepository.cs �r implementationen av IAnv�ndareRespository f�r
		kontakt mellan Anv�ndareTj�nst och databasen 


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
	IMinnetProdukterRepository.cs �r en mock repository f�r testning som implementerar
		IProdukterRepository

Controllers ->
	HemControllerTest.cs inneh�ller testar f�r huvudsidan (automatgenererad �n s� l�nge)

	ProduktControllerTest.cs inneh�ller metoder H�mtaProdukt och ProduktController, och
		inreklassen MockHttpContext, f�r att g�ra allt redo f�r testning.  Den inneh�ller
		testar f�r att r�tt vyn visas f�r produktlistan och produkten.

	ProduktTj�nstTest.cs inneh�ller metoder H�mtaProdukt och ProduktTj�nst f�r att g�ra
		allt redo f�r testning.  Den inneh�ller testar f�r att r�tt lista av produkter
		returneras. 

F�r �ndringar sedan sist ser Changelog.txt

