#include "InterfacciaConsole.h"
#include <iostream>
#include <iomanip>
#include <limits>

using namespace std;

InterfacciaConsole::InterfacciaConsole(Orario& orario, ElencoClassi& elencoClassi)
    : orario(orario), elencoClassi(elencoClassi) {}

int InterfacciaConsole::LeggiIntero(string messaggio, int minimo, int massimo) {
    int valore;
    while (true) {
        cout << messaggio;
        if (cin >> valore && valore >= minimo && valore <= massimo) return valore;
        if (cin.eof()) return minimo; // Evita un ciclo infinito con input terminato.
        cout << "Valore non valido. Inserisci un numero tra "
             << minimo << " e " << massimo << ".\n";
        cin.clear();
        cin.ignore(numeric_limits<streamsize>::max(), '\n');
    }
}

Posizione InterfacciaConsole::LeggiPosizione() {
    int giorno = LeggiIntero("Giorno (1=Lun, 2=Mar, 3=Mer, 4=Gio, 5=Ven): ", 1, 5);
    int ora = LeggiIntero("Ora (1-8): ", 1, 8);
    return Posizione(giorno - 1, ora - 1);
}

void InterfacciaConsole::VisualizzaOrario() {
    string giorni[5] = {"Lun", "Mar", "Mer", "Gio", "Ven"};
    cout << "\nORARIO SETTIMANALE DEL DOCENTE\n";
    cout << left << setw(6) << "Ora";
    for (int giorno = 0; giorno < 5; giorno++) cout << setw(22) << giorni[giorno];
    cout << "\n";
    for (int ora = 0; ora < 8; ora++) {
        cout << left << setw(6) << ora + 1;
        for (int giorno = 0; giorno < 5; giorno++) {
            Tessera* tessera = orario.Get(Posizione(giorno, ora));
            if (tessera == nullptr) cout << setw(22) << "---";
            else cout << setw(22) << (tessera->GetMateria() + " " + tessera->GetClasse());
        }
        cout << "\n";
    }
    cout << "\nPer visualizzare anche l'aula, scegli l'opzione 2.\n";
}

void InterfacciaConsole::VisualizzaTessera() {
    cout << "\nCONSULTA UNA CELLA\n";
    Posizione posizione = LeggiPosizione();
    Tessera* tessera = orario.Get(posizione);
    if (tessera == nullptr) cout << "La cella e' vuota.\n";
    else {
        cout << "Materia: " << tessera->GetMateria() << "\n";
        cout << "Classe: " << tessera->GetClasse() << "\n";
        cout << "Aula: " << tessera->GetAula() << "\n";
    }
}

void InterfacciaConsole::RichiediScambio() {
    cout << "\nSCAMBIO DI DUE CELLE\nPrima posizione:\n";
    Posizione prima = LeggiPosizione();
    cout << "Seconda posizione:\n";
    Posizione seconda = LeggiPosizione();
    if (prima.GetGiorno() == seconda.GetGiorno() &&
        prima.GetOra() == seconda.GetOra()) {
        cout << "Hai selezionato due volte la stessa cella.\n";
        return;
    }
    if (motore.Scambia(orario, prima, seconda)) cout << "Scambio autorizzato.\n";
    else cout << "Scambio rifiutato: l'orario non rispetterebbe il vincolo senza buchi.\n";
}

void InterfacciaConsole::VerificaOrario() {
    cout << "\nVERIFICA DELL'ORARIO\n";
    if (validatore.SenzaBuchi(orario)) cout << "Orario valido: non ci sono buchi.\n";
    else cout << "Orario non valido: sono presenti ore vuote prima di altre lezioni.\n";
}

void InterfacciaConsole::MostraMenu() {
    cout << "\n================================\n";
    cout << "       ORARIO DEL DOCENTE\n";
    cout << "================================\n";
    cout << "1 - Visualizza orario\n";
    cout << "2 - Visualizza una tessera\n";
    cout << "3 - Scambia due tessere\n";
    cout << "4 - Verifica validita' dell'orario\n";
    cout << "5 - Visualizza elenco classi\n";
    cout << "6 - Cerca una classe\n";
    cout << "7 - Aggiungi una classe\n";
    cout << "0 - Esci\n";
}

void InterfacciaConsole::Esegui() {
    int scelta;
    do {
        MostraMenu();
        scelta = LeggiIntero("Scelta: ", 0, 7);
        switch (scelta) {
        case 1: VisualizzaOrario(); break;
        case 2: VisualizzaTessera(); break;
        case 3: RichiediScambio(); break;
        case 4: VerificaOrario(); break;
        case 5: VisualizzaClassi(); break;
        case 6: CercaClasse(); break;
        case 7: AggiungiClasse(); break;
        case 0: cout << "Chiusura del programma.\n"; break;
        }
    } while (scelta != 0);
}

void InterfacciaConsole::VisualizzaClassi() {
    cout << "\nCLASSI DEL DOCENTE\n";
    for (int i = 0; i < elencoClassi.GetNumeroClassi(); i++) {
        const Classe* classe = elencoClassi.Get(i);
        cout << classe->GetSigla() << " - " << classe->GetIndirizzo()
             << " - studenti: " << classe->GetNumeroStudenti() << "\n";
    }
}

void InterfacciaConsole::CercaClasse() {
    string sigla;
    cout << "Sigla da cercare: ";
    if (!(cin >> sigla)) return;
    int indice = elencoClassi.Cerca(sigla);
    if (indice == -1) {
        cout << "Classe non presente.\n";
        return;
    }
    const Classe* classe = elencoClassi.Get(indice);
    cout << "Trovata: " << classe->GetSigla()
         << " - " << classe->GetIndirizzo()
         << " - studenti: " << classe->GetNumeroStudenti() << "\n";
}

void InterfacciaConsole::AggiungiClasse() {
    string sigla, indirizzo;
    cout << "Sigla: ";
    if (!(cin >> sigla)) return;
    cout << "Indirizzo (una parola): ";
    if (!(cin >> indirizzo)) return;
    int numero = LeggiIntero("Numero studenti (1-40): ", 1, 40);
    if (elencoClassi.Aggiungi(Classe(sigla, indirizzo, numero)))
        cout << "Classe aggiunta all'archivio.\n";
    else
        cout << "Inserimento rifiutato: sigla vuota o gia' presente.\n";
}
