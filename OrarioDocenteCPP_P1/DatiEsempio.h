#pragma once
#include "Model/Orario.h"
#include "Model/ElencoClassi.h"

class DatiEsempio {
public:
    // Carica un orario inizialmente vuoto. Restituisce false in caso di errore.
    static bool Carica(Orario& orario);
    static bool CaricaClassi(ElencoClassi& elenco);
};
