# Zapisky ze setkani 14.11.2024

## Co noveho:

- pridany custom modaly, abych mel vice kontroly (bohuzel to nepomohlo s nastavovanim z-indexu pred overlay kvuli stacking contextu)
- zjistil jsem, ze lze nastavit overlay pres bootstrap modalu
- pridana funkce na detekce toho, zda je nejaka modala aktivni (funguje i s custom modalami)

- pridani druheho ukolu, jakmile opravim prvni chybu
  - prilis prisny validator

- vyhnout se modale pres modalu -> pred vypsanim vysledku hledani chyby zavrit vsechny modaly

- napad od segry
  - filtrovani ucastniku podle alergenu by melo spise filtrovat podle or -> vim, ze budu varit nejake jidlo s danymi alergeny a chci vedet, kterym lidem by tohle mohlo ublizit
    - pridat ukol, kde bude presne toto jako problem -> najdi vsechny lidi, kterym nebude mozne dat k obedu dane jidlo

- dokonce mi segra nasla chybu: vek nelze zadat desetinne cislo, ale moje chybove hlasky to prechytraci

- celkem hezky lze highlightovat i buttonky

## Co poresit:

- na priste lepe zkategorizovat chyby?
- jak moc je dulezite udrzet modaly misto mozna jednodussiho nahrazeni separatnimi strankami?
- vyresit, jake chyby implementovat jako dalsi?

## Future tasks:

- pomalu resit sepisovani textu
  - sablona 
  - osnova
  - latex - extension do vscode