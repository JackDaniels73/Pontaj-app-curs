#---------- Sistem de Pontaj si Gestiune a Platilor pentru Cursuri ----------

#---------- Descrierea Proiectului ----------

#Aceasta este o aplicatie dezvoltata in C# destinata instructorilor care sustin cursuri saptamanale pentru copii. 
#Rolul aplicatiei este de a tine evidenta prezentelor si de a monitoriza automat statusul platilor pentru fiecare cursant.

  #Operatii pe care doresc sa le implementez:
## Adaugarea cursantilor: Inregistrarea copiilor in sistem cu un nume si un sold initial de sedinte platite.

## Inregistrarea prezentei: Marcarea prezentei saptamanale. Fiecare prezenta va scadea automat o sedinta din "contul" cursantului.

## Actualizarea platilor: Adaugarea de noi sedinte platite in avans atunci cand parintii fac o plata.

## Verificarea situatiei financiare: Afisarea unei liste cu toti cursantii, care va indica automat:

  ### Sedinte ramase: Cate cursuri mai poate urma copilul pe baza platii in avans.
  ### Plata necesara: Un mesaj de avertizare in cazul in care soldul de sedinte ajunge la 0.
