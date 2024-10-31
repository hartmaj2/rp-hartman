- overovani splneni ukolu vyresime pozdeji
- hlaseni chyb bude mozne i vyberem z dropdownu


- realne chyby vs chyby, ktere by udelal nekdo nezkuseny

## Bestiar chyb
- alergie by mela zamezit objednavce jidla s alergenem
- **spatne trideni**
- spatne filtrovani
- spatny format data
- error 404 - stranka neexistuje 
- spatny stav - program spadnul
- spatny font 
- jmeno a prijmeni neoddelene - nelze tridit jen podle jednoho
- prijmeni brano jako id - uprava cloveka upravi i toho druheho se stejnym prijmenim
- orezani dat, ktere se nevejdou do tabulky

## Kategorie chyb

### UI Chyby - Ne user friendly design
- spatne citelny text 
- elementy schovane za sebou
- tlacitko mimo dosah
- nepta se na potvrzeni volby
- nejednotny design
- neco upravit je moc slozite 

### Validacni chyby
- zadavani hodnot souvisejicich na jinych hodnotach (logicke souvislosti/vazby)
  - alergie by mela zamezit objednavce jidla s alergenem
  - nemoznost pridat ucastnika do plne chatky
  - **nemoznost pridat ucastnika na tabor pokud uz je vice ucastniku nez celkova kapacita chatek dohromady**
- **jednoducha validace**
  - prilis volna / prilis prisna
  - povinne pole
  - platne datum

### Chyby v technickem provedeni (z nepozornosti programatora)
- nefungujici tlacitka
- neupravujici se pocty (objednavky)
- **neexistujici tlacitko, ktere je potreba k nejakemu ukonu** (nahlasovani mozna vybranim osviceneho mistecka tam, kde by to tlacitko melo byt)
- navrh tabulky
  - **navrh tabulek neumoznuje vybrat vice nez 2 alegreny**

### Chyba v zachazeni s daty

### Chyby v zabezpeceni
- soukromi
  - lze videt osobni data ostatnich v nejake tabulce
- moznost udelat nejakou akci za nekoho jineho
  - **objednavani pokrmu zapsanim jmena cloveka**



## TODO:
- [ ] vybrat naimplementovatelne chyby ze seznamu vyse
- [ ] revidovat typy chyb a mozne chyby 
  - [ ] rozdelit do kategorii
- [ ] vybrat chyby, ktere pouzijeme
  - dobre naimplementovatelne
  - ne moc komplexni 
  - nejlepe z ruchnych kategorii 
  - pripadne rozmyslet navazujici minihru
- [ ] v jakem poradi dame oznacene chyby
