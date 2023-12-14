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
