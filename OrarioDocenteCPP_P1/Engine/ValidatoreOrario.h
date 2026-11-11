#pragma once
#include "../Model/Orario.h"

class ValidatoreOrario {
public:
    // Controlla che non vi siano ore vuote tra due lezioni dello stesso giorno.
    bool SenzaBuchi(Orario& orario);
};
