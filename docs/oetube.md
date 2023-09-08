# OETube - Youtube+Udemy klón az OE számára

## Leírás

A feladat egy olyan OE közösségi videóportál létrehozása, ahol bármely oktató vagy hallgató tud videókat feltölteni és ezekből akár listákat készíteni.


### Működés

Kizárólag O365 bejelentkezéssel működjön az oldal vagy anonim módon csökkentett funkciókkal. Adott videóknál vagy komplett listáknál beállítható, hogy ki tekintheti meg 
(csak én, bárki, bárki OE-s, csoport). A videók átalakítása kliensoldalon a böngészőben történjen. Hogyha ez nem támogatott, akkor a szerver oldja meg. De mindkét esetben a konvertálás állapotát lehessen a böngészőben követni. A videókból keletkezetten 1080p, 720p és 480p felbontású.


### Böngésző nézet

Itt érhető el minden funkció. Akár a szerkesztő felület lehet egy külön app (pl. editor.oetube) és a megjelenítő felület is egy külön app. Ezt a csapat döntheti el.

### Mobil nézet

Szükséges egy telepíthető mobilalkalmazás készítése is. Itt viszont csak a lejátszás+böngészés implementálása az elvárt. Ez a 2023/24/2 félévben elég. 


### Biztonsági intézkedések

A videókat ne lehessen letölteni. Szerveroldalról HLS streamként kell megoldani a közvetítést. Illetve a chunkok letöltéséhez a headerben kell tokent küldeni.
