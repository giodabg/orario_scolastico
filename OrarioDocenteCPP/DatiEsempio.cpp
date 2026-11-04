#include "DatiEsempio.h"

bool DatiEsempio::Carica(Orario& orario) {
    const std::string classi[4] = {"4AI", "4BI", "3AI", "5AI"};
    const std::string materie[2] = {"Informatica", "Laboratorio"};

    for (int giorno = 0; giorno < 5; giorno++) {
        int numeroLezioni = (giorno == 4) ? 6 : 7;
        for (int ora = 0; ora < numeroLezioni; ora++) {
            std::string classe = classi[(giorno + ora) % 4];
            std::string materia = materie[(giorno + ora) % 2];
            std::string aula = "A" + std::to_string(ora + 1);
            Tessera* tessera = new Tessera(materia, classe, aula);
            if (!orario.Set(Posizione(giorno, ora), tessera)) {
                delete tessera; // Set non ha acquisito la proprieta'.
                return false;
            }
        }
    }
    return true;
}
