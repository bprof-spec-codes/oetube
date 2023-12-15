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
  A kiválasztott videót lehet megnézni, illetve a videó jobb oldalán minden egyéb videó klistázásra kerül, amihez a felhasználónak jogosultsága van
  **HIÁNYZIK A VIDEÓ JOBB OLDALÁRÓL AZ ÖSSZES TÖBBI VIDEÓ**
  Amennyiben egy lejátszási listához tartozik a videó, akkor a jobb oldalon az adott lejátszási többi tartalma jelenik meg.
  **A LEJÁTSZÁSI LIST JOBB OLDALA LEVÁGÓDIK, EZT MÉG JAVÍTANI KELL**


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
  A lejátszási lista minden paramétere edit-elhető.
  **HIÁNYZIK A PLAYLIST EDIT FELÜLET**


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
  **MÉG NEM MŰKÖDIK A GROUP MEMBERS**

  #### Group - Edit
  A kiválasztott csoport szerkeszthető.
  **MÉG NEM MŰKÖDIK A GROUP EDIT**




# Probléma jegyzőkönyv
## Backend
## Frontend
 - Az abp és a devex megjelenítés összeolvasztása nem volt zökkenőmentes
 - Videó lejátszásánál kezelni kellett azt, hogy a video seeker egyszerre mutassa azt, hogy hol tart a jelenlegi lejátszás, és kézzel is állítható legyen (nehéz volt megkülönböztetni, hogy a seeker állása azért változott meg, mert a videó előrehaladt, vagy mert a felhasználó beletekert)
 - Többszöri videómegnyitáskor nem állt le az előző videó, ezért többszörösen is hallani lehetett a hangot.
