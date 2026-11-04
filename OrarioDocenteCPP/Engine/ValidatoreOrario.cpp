#include "ValidatoreOrario.h"

bool ValidatoreOrario::SenzaBuchi(Orario& orario) {
    // Un buco e' una cella vuota prima di una lezione dello stesso giorno.
    // Le ore libere alla fine della giornata sono ammesse.
    for (int giorno = 0; giorno < 5; giorno++) {
        bool trovataOraLibera = false;
        for (int ora = 0; ora < 8; ora++) {
            if (orario.Get(Posizione(giorno, ora)) == nullptr)
                trovataOraLibera = true;
            else if (trovataOraLibera)
                return false;
        }
    }
    return true;
}
