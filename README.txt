**Jag tror INTE det kr�ver n�got �ndring s� databasen fungerar p� andra datorer men jag
�r inte 100% s�ker.


Huvudsidan har Controllers -> HemController.cs som kodfil
och Views -> Hem -> Hem.cshtml som vy

Produkter har Controllers -> ProdukterController.cs som kodfil
och Views -> Produkter -> Index.cshtml som vy
(Create, Delete, och Edit var automatgenererad men kommer inte att anv�ndas)

Views -> Produkter -> Details.cshtml �r vyn f�r produktdetaljer

Views -> Produkter -> Prislista.cshtml �r vyn f�r pristlistan

LOMDB_Model.edmx �r automatgenererad modellen av databasen LOM_DBEntities

Anvandare �r tabellen f�r inloggningssystemet och �r inte kopplad till n�got �n.
Vi beh�ver inte anv�nda oss av den om vi hittar en annan l�sning.
Controllers -> AnvandareController.cs �r koden
och Views -> Anvandare -> (automatgenererad Create, Delete, Details, Edit, Index.cshtml)

LjusOchMiljoAB.Tests bara inneh�ller n�gra automatgenererad tester �n s� l�nge. 

F�r �ndringar sedan sist, ser Changelog.txt


...

�vriga automatgenererad filer av betydelse

Som default startas ett MVC basprojekt med registrering och inloggningsformer
- Controllers -> AccountController
- Controllers -> ManageController
- Views -> Account -> en massa vyn f�r registrering och login
- Views -> Manage -> form f�r att byta l�senord, mm

Den har �ven vyer som �r delad f�r alla vyn (meny)
- Views -> Shared -> _Layout.cshtml (meny layout)
- Views -> Shared -> _LoginPartial.cshtml (som visar om man �r inloggad eller inte)
- Views -> Shared -> Error.cshtml (n�got gick snett)
- Views -> Shared -> Lockout.cshtml (banad konto)

App_Start mappen inneh�ller n�gra configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med l�nkar)
- FilterConfig.cs �r en inbyggt datafilter
