#include <iostream>
#include <iomanip>
#include <limits>
#include <string>

#include "Model/Orario.h"
#include "Engine/MotoreScambi.h"
#include "Engine/ValidatoreOrario.h"

using namespace std;

// Legge un intero, ripetendo la richiesta se l'utente digita un valore non valido.
int LeggiIntero(string messaggio, int minimo, int massimo) {
    int valore;
    while (true) {
        cout << messaggio;
        if (cin >> valore && valore >= minimo && valore <= massimo) {
            return valore;
        }
        cout << "Valore non valido. Inserisci un numero tra "
             << minimo << " e " << massimo << ".\n";
        cin.clear();
        cin.ignore(numeric_limits<streamsize>::max(), '\n');
    }
}

// Il menu usa numeri da 1; la matrice usa indici da 0.
Posizione LeggiPosizione() {
    int giorno = LeggiIntero("Giorno (1=Lun, 2=Mar, 3=Mer, 4=Gio, 5=Ven): ", 1, 5);
    int ora = LeggiIntero("Ora (1-8): ", 1, 8);
    return Posizione(giorno - 1, ora - 1);
}

void CaricaOrarioEsempio(Orario& orario) {
    string materie[7] = {
        "Matematica", "Informatica", "Italiano", "Inglese",
        "Sistemi", "TPS", "Storia"
    };

    for (int giorno = 0; giorno < 5; giorno++) {
        int numeroLezioni = 7;
        if (giorno == 4) {
            numeroLezioni = 6;
        }

        for (int ora = 0; ora < numeroLezioni; ora++) {
            string materia = materie[(giorno + ora) % 7];
            string aula = "A" + to_string(ora + 1);
            Tessera* tessera = new Tessera(materia, aula);

            // La cella e' inizialmente vuota: Set trasferisce la proprieta'
            // della tessera all'orario quando restituisce true.
            bool inserita = orario.Set(Posizione(giorno, ora), tessera);
            if (!inserita) {
                // Situazione inattesa: il caricamento deve usare solo celle vuote.
                // L'indirizzo resta al chiamante: non perderlo se modifichi il codice.
                cout << "Errore nel caricamento della cella "
                     << giorno + 1 << ", " << ora + 1 << ".\n";
            }
        }
    }
}

void VisualizzaOrario(Orario& orario) {
    string giorni[5] = {"Lun", "Mar", "Mer", "Gio", "Ven"};

    cout << "\nORARIO SETTIMANALE\n";
    cout << left << setw(6) << "Ora";
    for (int giorno = 0; giorno < 5; giorno++) {
        cout << setw(18) << giorni[giorno];
    }
    cout << "\n";

    for (int ora = 0; ora < 8; ora++) {
        cout << left << setw(6) << ora + 1;
        for (int giorno = 0; giorno < 5; giorno++) {
            Tessera* tessera = orario.Get(Posizione(giorno, ora));
            if (tessera == nullptr) {
                cout << setw(18) << "---";
            } else {
                cout << setw(18) << tessera->GetMateria();
            }
        }
        cout << "\n";
    }
    cout << "\nPer visualizzare anche l'aula, scegli l'opzione 2.\n";
}

void VisualizzaTessera(Orario& orario) {
    cout << "\nCONSULTA UNA CELLA\n";
    Posizione posizione = LeggiPosizione();
    Tessera* tessera = orario.Get(posizione);

    if (tessera == nullptr) {
        cout << "La cella e' vuota.\n";
    } else {
        cout << "Materia: " << tessera->GetMateria() << "\n";
        cout << "Aula: " << tessera->GetAula() << "\n";
    }
}

void RichiediScambio(Orario& orario, MotoreScambi& motore) {
    cout << "\nSCAMBIO DI DUE CELLE\n";
    cout << "Prima posizione:\n";
    Posizione prima = LeggiPosizione();
    cout << "Seconda posizione:\n";
    Posizione seconda = LeggiPosizione();

    if (prima.GetGiorno() == seconda.GetGiorno() &&
        prima.GetOra() == seconda.GetOra()) {
        cout << "Hai selezionato due volte la stessa cella.\n";
        return;
    }

    if (motore.Scambia(orario, prima, seconda)) {
        cout << "Scambio autorizzato.\n";
    } else {
        cout << "Scambio rifiutato: si creerebbe un'ora vuota "
             << "prima di una lezione.\n";
    }
}

void VerificaOrario(Orario& orario, ValidatoreOrario& validatore) {
    cout << "\nVERIFICA DELL'ORARIO\n";
    if (validatore.SenzaBuchi(orario)) {
        cout << "Orario valido: non ci sono buchi.\n";
    } else {
        cout << "Orario non valido: sono presenti ore vuote "
             << "prima di altre lezioni.\n";
    }
}

void MostraMenu() {
    cout << "\n================================\n";
    cout << "       ORARIO SCOLASTICO\n";
    cout << "================================\n";
    cout << "1 - Visualizza orario\n";
    cout << "2 - Visualizza una tessera\n";
    cout << "3 - Scambia due tessere\n";
    cout << "4 - Verifica validita' dell'orario\n";
    cout << "0 - Esci\n";
}

int main() {
    Orario orario;
    MotoreScambi motore;
    ValidatoreOrario validatore;

    CaricaOrarioEsempio(orario);

    int scelta;
    do {
        MostraMenu();
        scelta = LeggiIntero("Scelta: ", 0, 4);

        switch (scelta) {
        case 1:
            VisualizzaOrario(orario);
            break;
        case 2:
            VisualizzaTessera(orario);
            break;
        case 3:
            RichiediScambio(orario, motore);
            break;
        case 4:
            VerificaOrario(orario, validatore);
            break;
        case 0:
            cout << "Chiusura del programma.\n";
            break;
        }
    } while (scelta != 0);

    return 0;
}
