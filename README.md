# Proiect3

Creați o aplicație care sa conțină o baza de date creata in MS SQL SERVER si o interfața pentru aceasta. La crearea interfeței se va folosi tehnologia .NET.

Baza de date va fi compusa din următoarele tabele:
• Medic(MedicID, NumeMedic, prenumeMedic, Specializare);
• Pacient(PacientID, CNP, NumePacient, PrenumePacient,Adresa, Asigurare);
• Medicamente(MedicamentID, Denumire);

Asocierile intre tabele sunt următoarele:
• Intre tabela Medic si tabela Pacient – raport de cardinalitate M:N.
• Intre tabela Pacient si tabela Medicament – raport de cardinalitate M:N. 

In acest caz, tabela de legătură se va numi Consultatie; astfel, pe lângă atributele necesare realizării multiplicității M:N, vor mai fi adăugate următoarele: Data, Diagnostic, DozaMedicament.
Interfața va trebui sa permită utilizatorului sa facă următoarele operații pe toate tabelele: vizualizare, adăugare, modificare, ștergere. Vizualizarea tabelelor de legătură va presupune vizualizarea datelor referite din celelalte tabele.
