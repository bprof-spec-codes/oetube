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
