#include <iostream>
#include "Model/Orario.h"
#include "DatiEsempio.h"
#include "InterfacciaConsole.h"

int main() {
    Orario orario;
    if (!DatiEsempio::Carica(orario)) {
        std::cout << "Errore nel caricamento dell'orario di esempio.\n";
        return 1;
    }
    ElencoClassi elencoClassi;
    if (!DatiEsempio::CaricaClassi(elencoClassi)) {
        std::cout << "Errore nel caricamento delle classi di esempio.\n";
        return 1;
    }
    InterfacciaConsole interfaccia(orario, elencoClassi);
    interfaccia.Esegui();
    return 0;
}
