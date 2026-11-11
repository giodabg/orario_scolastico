#include "ValidatoreOrario.h"

bool ValidatoreOrario::SenzaBuchi(Orario& orario) {
    for (int giorno = 0; giorno < 5; giorno++) {
        bool lezioneIniziata = false;
        bool bucoTrovato = false;

        for (int ora = 0; ora < 8; ora++) {
            if (orario.Get(Posizione(giorno, ora)) != nullptr) {
                // Una lezione dopo una cella vuota interna crea un buco.
                if (bucoTrovato) return false;
                lezioneIniziata = true;
            }
            else if (lezioneIniziata) {
                // Potrebbe essere un buco oppure la fine delle lezioni.
                bucoTrovato = true;
            }
        }
    }
    return true;
}
