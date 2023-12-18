# Csapatbeosztás
- Architect: Török Tamás
- SCRUM Master: Szanyi Szabolcs
- Product Owner: Szalai István
- Fejlesztő: Béres Benjámin
- Fejlesztő: Schaffer Tamás
- Fejlesztő: Molnár Ákos

# User manual
## Üzemeltetés
<strong>Backend</strong> elindításához szükségünk van egy Visual Studio-ra vagy bármilyen IDE-ra vagy kód editorra.

<strong>Frontend</strong> elindításához szükségünk van egy Angular CLI-re. Amennyiben először húzzuk le a repository-t, akkor szükség van az "npm install" parancsra, majd ezt követően pedig az "ng serve" utasítást kiadva a kliensünk le build-elődik. Ha nem adtunk meg egyéb portot, akkor a klienst alapértelmezetten a http://localhost:4200 címen érjük el.

## User és Group információk
**User**
+ TestAdmin1
	+ Email: TestAdmin1@uni-obuda.hu
	+ Password: TestAdmin1!
	+ Role: Admin
+ TestAdmin2
	+ Email: TestAdmin2@stud.uni-obuda.hu
	+ Password: TestAdmin2! 
	+ Role: Admin
+ TestUser1
	+ Email: TestUser1@stud.uni-obuda.hu
	+ Password: TestUser1!
+ TestUser2
	+ Email: TestUser2@uni-obuda.hu
	+ Password: TestUser2!
+ TestUser3
	+ Email: TestUser3@stud.uni-obuda.hu
	+ Password: TestUser3!
+ TestUser4
	+ Email: TestUser4@gmail.com
	+ Password: TestUser4!
+ TestUser5
	+ Email: TestUser5@gmail.com
	+ Password: TestUser5!

**Group**
+ Oe
	+ Creator: TestAdmin1
	+ EmailDomains: uni-obuda.hu, stud.uni-obuda.hu
+ OeStud
	+ Creator: TestAdmin2
	+ EmailDomains: stud.uni-obuda.hu
+ Random
	+ Creator: TestUser2
	+ Members: TestUser4, TestUser5, TestUser3
+ Empty
	+ Creator: TestUser1

## API funkciólista
+ #### /Group ####
	+ **Get**
		+ **/api/app/group:** *Pagináltan lekérdezo az összes Group-t a megadott keresési argumentumok alapján.*
		+ **/api/app/group/{id}:** *Lekérdez egy Group-t id alapján.*
		+ **/api/app/group/{id}/explicit-members:** *Megadja az adott id-jű Grouphoz tartozó Memberként hozzáadott Userek paginált listáját a keresési argumentumok alapján.*
		+ **/api/app/group/{id}/group-members:** *Megadja az adott id-jű Grouphoz tartozó összes User paginált listáját, beleértve a Membereket és az EmailDomaint is, a keresési argumentumok alapján.*
		+ **/api/src/group/{id}/image:** *Lekéri az adott id-jű Group képét.*
		+ **/api/src/group/{id}/thumbnail-image:** *Lekéri az adott id-jű Group thumbnail képét.*
		+ **/api/src/group/default-image:** *Lekéri a Groupok alapértelmezett képét.*
	+ **Post**
		+ **/api/app/group:** *Létrehoz egy Group-t. Csak bejelentkezett felhasználó hozhat létre.*
		+ **/api/app/group/upload-default-image:** *Feltölt egy alapértelmezett képet a Groupokhoz. Csak admin jogosultságú felhasználó hajthatja végre.*
	+ **Put**
		+ **/api/app/group/{id}:** *Módosít egy Group-t. Csak a létrehozója hajthatja végre.*
	+ **Delete**
		+ **/api/app/group/{id}:** *Kitöröl egy Group-t. Csak a létrehozója hajthatja végre.*
+ #### /OeTubeUser ####
	+ **Get**
		+ **/api/app/oe-tube-user:** *Pagináltan lekérdez több User-t a megadott keresési argumentumok alapján.*
		+ **/api/app/oe-tube-user/{id}:** *Lekérdez egy User-t id alapján.*
		+ **/api/app/oe-tube-user/{id}/groups:** *Pagináltan lekérdezi az összes Grouput, amihez User tartozik a keresési argumentumok alapján.*
		+ **/api/src/oe-tube-user/default-image:** *Lekéri a Userek alapértelmezett képét.*
		+ **/api/src/ou-tube-user/{id}/image:** *Lekéri az adott id-jű User képét.*
		+ **/api/src/ou-tube-user/{id}/thumbnail-image:** *Lekéri az adott id-jű User thumbnail képét.*
	+ **Post**
		+ **/api/app/oe-tube-user/upload-default-image:** *Feltölt egy alapértelmezett képet a Userekhez. Csak admin jogosultságú felhasználó hajthatja végre.*
	+ **Put**
		+ **/api/app/oe-tube-user/{id}:** *Módosít egy User profilját. Csak a hozzátartozó felhasználó hajthatja végre.*
+ #### /Playlist ####
	+ **Get**
		+ **/api/app/playlist:** *Pagináltan lekérdezi az összes kérelmezőnek elérhető Playlist-t a megadott keresési argumentumok alapján.*
		+ **/api/app/playlist/{id}:** *Lekérdez egy Playlist-t id alapján és ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/app/playlist/{id}/videos:** *Lekérdezi pagináltan az összes adott id-jű Playlisthez tartozó, kérelmezőnek elérhető videót a keresési argumentumok alapján.*
		+ **/api/src/playlist/{id}/image:** *Lekéri az adott id-jű Playlist képét és ellenőrzi, hogy a kérelmezőnek elérhető-e.*
		+ **/api/src/playlist/{id}/thumbnail-image:** *Lekéri az adott id-jű Playlist thumbnail képét és ellenőrzi, hogy a kérelmezőnek elérhető-e.*
		+ **/api/src/playlist/default-image:** *Lekéri a Playlistek alapértelmezett képét.*
	+ **Post**
		+ **/api/app/playlist:** *Létrehoz egy Playlist-t. Csak bejelentkezett felhasználó hozhat létre és csak saját videókat adhat hozzá, mint elem.*
		+ **/api/app/playlist/upload-default-image:** *Feltölt egy alapértelmezett képet a Playlistekhez. Csak admin jogosultságú felhasználó hajthatja végre.*
	+ **Put**
		+ **/api/app/playlist/{id}:** *Módosit egy Playlist-t. Csak létrehozója hajthatja végre és csak saját videókat adhat hozzá, mint elem.*
	+ **Delete**
		+ **/api/app/playlist/{id}:** *Kitöröl egy Playlist-t. Csak a létrehozója hajthatja végre.*
+ #### /Video ####
	+ **Get**
		+ **/api/app/video:** *Pagináltan lekérdezi az összes kérelmezőnek elérhető Video-t a megadott keresési argumentumok alapján.*
		+ **/api/app/video/{id}:** *Lekérdez egy Video-t id alapján és ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/app/video/{id}/access-groups:** *Lekérdezi pagináltan az összes adott id-jű Video-hoz tartozó Group-t a keresési argumentumok alapján és ellenörzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/app/video/{id}/index-images:** *Lekérdezi az adott id-jű Video-hoz tartozó indexképeket. Csak a létrehozója fér hozzá.*
		+ **/api/src/video/{id}/{width}x{height}/{segment}.ts:** *Lekéri az adott id-jű Video-hoz tartozó HLS szegmenst felbontás(widthxheight) és sorszám(segment) alapján. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/src/video/{id}/{width}x{height}/list.m3u8:** *Lekéri az adott id-jű Video-hoz tartozó HLS listát felbontás(widthxheight) alapján. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/src/video/{id}/index_image:** *Lekéri az adott id-jű Video-hoz tartozó, beállított indexképet. Ellenőrzi, hogy a kérelmezőnek van-e hozzáférése.*
		+ **/api/src/video/{id}/index_image/{index}:** *Lekéri az adott id-jű Video-hoz tartozó indexképet index alapján. Csak a Video létrehozója fér hozzá.*
	+ **Post**
		+ **/api/app/video/{id}/continue-upload:** *Feltölti a kliens által átméretezett fájlt az adott id-jű Video-hoz és leellenörzi a tartalmát. Ha az összes kiosztott feladat teljesült a szerver elkezdi feldolgozni a feltöltéseket. Csak a Video létrehozója folytathatja a feltöltést és csak mp4 formátum támogatott.*
		+ **/api/app/video/start-upload:** *Feltölti a forrásfájlt és ellenőrzés után a szerver kiosztja az átméretezési feladatokat a kliensnek. Csak bejelentkezett felhasználó tölthet fel és csak mp4 formátum támogatott.*
	+ **Put**
		+ **/api/app/video/{id}:** *Módosit egy Video-t. Csak a létrehozója hajthatja végre.*
	+ **Delete**
		+ **/api/app/video/{id}:** *Kitöröl egy Video-t. Csak a létrehozója hajthatja végre.*

## UI ismertető
  ### Video
  #### Video - Explore
  Bejelentkezéstől függetlenül hozzáfér a felhasználó ehhez az oldalhoz.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/00b4f8e7-8c3b-439a-9737-d62e4bb8683a)

  
  Bejelentkezett felhasználó nem csak a publikus videókhoz fér hozzá, hanem minden olyan videóhoz, amihez jogosultsága van. Az alábbi képen egy extra videóval bővül ki a lista, amely a felhasználóhoz tartozó privát videó. Továbbá megjelenik egy extra tab (Upload), ahol a felhasználónak lehetősége van feltölteni videót.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/fde7468f-e7fb-416a-ba84-e657c21b6a0e)


  #### Video - Upload
  Ezen a felületen lehet feltölteni a kiválasztott videó fájlt. A Name mező kitöltése kötelező, viszont a Description mező opcionális. Ezen két paraméteren kívül értelemszerűen ki kell választani a videófájlt a SELECT FILE gomb megnyomása után. 
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/1b5e9127-c3a2-42ba-9b47-b86a7626bf35)
  Van lehetőség a láthatóságot is kiválasztani (Private, Public, Group). A group esetében egyénileg elkészített csoportok közül lehet választani.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/a60affc2-21b4-4eee-9663-b5bac8fafcd7)

Amennyiben a felhasználó megnyomja a SUBMIT gombot, akkor a kovenrzió, majd feltöltés megkezdődik. A konverzió állapotát egy pop-up ablakon lehet nyomonkövetni.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/880058ee-8f9c-47a6-bec1-2c23a21c3df1)

  #### Video - Watch
  A kiválasztott videót lehet megnézni, illetve a videó jobb oldalán minden egyéb videó klistázásra kerül, amihez a felhasználónak jogosultsága van. Amennyiben egy lejátszási listához tartozik a videó, akkor a jobb oldalon az adott lejátszási többi tartalma jelenik meg.

  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/854cd2bf-4b6b-47b6-8ec3-8c932764a0c3)

  #### Video - Edit
  Ezen a felületen lehet módosítani a feltöltött videó adatait
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/cfbbcb30-1523-45aa-aa63-32fc12400c7d)

  ### Playlist

  #### Playlist - Explore
  Kilistázásra kerül minden playlist amihez hozzáfér a felhasználó. Amennyiben a felhasználó készítette a listát vagy van benne olyan tartalom amihez jogosultsága van a felhasználónak, akkor az a lejátszási lista megjelenik.
    ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/bcb7830e-e6b7-4ec5-a40f-f54080dfe363)

  #### Playlist - Create
  Lejátszási listákat lehet létrehozni, a megfelelő adatok megadása után. Ezen adatok közül a Name az kötelező, viszont a Description és a képfeltöltés opcionális. Amennyiben nem ad meg a felhasználó képet, akkor egy alapértelmezett kép, lesz a lejátszási lista indexképe. Létrehozáskor azonnal tudunk videókat is hozzáadni a listához. Ezeket a videókat egy pop-up ablakon keresztül tudjuk hozzáadni a listához.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/09198779-7b5f-4592-88f9-cbc78a94fd68)
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/7bf5ddc9-9bac-4823-8d69-8aee2aa7da1d)

  #### Playlist - Details
  A kiválaszott lejátszáis lista adatai (indexkép, name, description) és a tartalma jelenik meg.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/77c2ca2e-b0f3-4c2f-81f8-9313c95fc1a8)

  #### Playlist - Edit
  A lejátszási lista minden paramétere edit-elhető, illetve a Delete gomb megnyomásával törölhető a lista.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/a3bde3e6-b80e-4222-bab8-4121b9f49c0a)



  ### Group

  #### Group - Explore
  Az összes létrehozott csoport kilistázásra kerül és a a felhasználó által létrehozott csoportok szerkeszthetőek is.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/78c12aaa-1cc4-4624-bd26-6e00fae9c33a)

  #### Group - Create
  A Name paraméter kivételével minden egyéb paraméter opcionális. Fel lehet tölteni képet, viszont, ha nem tölt fel képet a felhasználó, akkor egy alapértelmezett indexkép rendelődik a csoporthoz. A megszokott stuktúra alapján egy pop-up ablakban lehet kiválasztani a csoporthoz tartozó felhasználókat. Ezen lépések utána SUBMIT gomb megnyomása után a csoport elkészül.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/536fa416-2bd0-4a89-ab73-6a97b51b700f)
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/e2206b9e-acfa-4843-9693-18d49d968545)
  
  #### Group - MEMBERS
  A kiválasztott csoport adatai jelennek meg, illetve az oda tartozó felhasználók kerülnek kilistázásra.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/8952fb01-6845-4015-971c-77b3bf1d9f00)


  #### Group - Edit
  A kiválasztott csoport szerkeszthető.
  ![image](https://github.com/bprof-spec-codes/oetube/assets/91885130/a9167134-41be-42cd-a4e8-dac44908dbff)

# Probléma jegyzőkönyv
## Backend
 - Új keretrendszer megismerése
 - Bejelentkezés hosszú ideig nem működött kliens oldalon, mert az environment.*.ts-ben az oAuthConfig "issuer" fieldjében a backend címének végéről lemaradt egy per jel.
 - ABP seedelés migráláskor rendszeresen hibára futott. A csoportok seedelésénél egy olyan több darabból álló(split query) lekérdezés volt használva, ami paginálta(skip,take) az adatokat, de nem határozta meg a rendezés módját. https://learn.microsoft.com/hu-hu/ef/core/querying/single-split-queries#split-queries.
 - Nem lehetett 3 karakternél hosszabb nevű lejátszási listát létrehozni, mert a playlist adatbázis konfigurációjában véletlenül fel volt cserélve a minimális és a maximális hossz.
 - Lejátszási listák videói nem az eredeti sorrendben jelentek meg. Módosításkor a kliens csak a videók id-jait küldi el, amikből egy  WHERE...IN-nek megfelelő LINQ lekérdezéssel jut hozzá a szerver a valódi entitásokhoz. Ezzel az a probléma, hogy az adatok lekérésének a sorrendje nem a bemeneti paraméterek sorendjétől függ, hanem ahogy épp az adatbázis "megtalálja" őket, szóval találatokat vissza kellett rendezni, mielőtt tovább lettek küldve update-re.


## Frontend
 - Az abp és a devex megjelenítés összeolvasztása nem volt zökkenőmentes
 - Videó lejátszásánál kezelni kellett azt, hogy a video seeker egyszerre mutassa azt, hogy hol tart a jelenlegi lejátszás, és kézzel is állítható legyen (nehéz volt megkülönböztetni, hogy a seeker állása azért változott meg, mert a videó előrehaladt, vagy mert a felhasználó beletekert)
 - Többszöri videómegnyitáskor nem állt le az előző videó, ezért többszörösen is hallani lehetett a hangot.
 - A csapatból valaki által létrehozott komponens újrafelhasználása nehézkes volt néha, mivel elég komplexekre sikeredtek és amennyiben nem te írtad, akkor nehézkes volt átlátni.
 -  Project kezdetén nagyon sok időbe telt, mire sikerült egy olyan FFMPEG WASM javascript könyvtárat találni, ami nem csak szerveroldali, hanem böngészős környezetben is működik: "@ffmpeg/core": "^0.11.0", "@ffmpeg/ffmpeg": "^0.11.5",
 -  Kliens kilistázta a videókat, de néhánynál az indexkép nem jelent és egyénileg sem lehetett elérni őket. 
Egyes videóknál a gond a hibás listázó SQL lekérdezés volt backend oldalon, így a felhasználónak olyan videókat is kilistázott, amihez valójában nem volt hozzáférése. Emellett voltak olyan videók is, amihez viszont volt és ugyanúgy nem jelentek meg a tartalmak. Itt a háttérben az állt, hogy a frontend a beágyazott tartalmaknál nem küldte el az azonosító tokent. Végül a képeknél egy pipe, míg a HLS streamingnél a helyes XMLHttpRequest konfiguráció megoldotta a problémát.

