**Jag tror INTE det kräver något ändring så databasen fungerar på andra datorer men jag
är inte 100% säker.


Huvudsidan har Controllers -> HemController.cs som kodfil
och Views -> Hem -> Hem.cshtml som vy

Produkter har Controllers -> ProdukterController.cs som kodfil
och Views -> Produkter -> Index.cshtml som vy
(Create, Delete, och Edit var automatgenererad men kommer inte att användas)

Views -> Produkter -> Details.cshtml är vyn för produktdetaljer

Views -> Produkter -> Prislista.cshtml är vyn för pristlistan

LOMDB_Model.edmx är automatgenererad modellen av databasen LOM_DBEntities

Anvandare är tabellen för inloggningssystemet och är inte kopplad till något än.
Vi behöver inte använda oss av den om vi hittar en annan lösning.
Controllers -> AnvandareController.cs är koden
och Views -> Anvandare -> (automatgenererad Create, Delete, Details, Edit, Index.cshtml)

LjusOchMiljoAB.Tests bara innehåller några automatgenererad tester än så länge. 

För ändringar sedan sist, ser Changelog.txt


...

Övriga automatgenererad filer av betydelse

Som default startas ett MVC basprojekt med registrering och inloggningsformer
- Controllers -> AccountController
- Controllers -> ManageController
- Views -> Account -> en massa vyn för registrering och login
- Views -> Manage -> form för att byta lösenord, mm

Den har även vyer som är delad för alla vyn (meny)
- Views -> Shared -> _Layout.cshtml (meny layout)
- Views -> Shared -> _LoginPartial.cshtml (som visar om man är inloggad eller inte)
- Views -> Shared -> Error.cshtml (något gick snett)
- Views -> Shared -> Lockout.cshtml (banad konto)

App_Start mappen innehåller några configurationsfiler
- RouteConfig.cs hanterar giltiga addresser (som man inte kommer till med länkar)
- FilterConfig.cs är en inbyggt datafilter
