#pragma once
#include <string>
#include "Model/Orario.h"
#include "Engine/MotoreScambi.h"
#include "Engine/ValidatoreOrario.h"

class InterfacciaConsole {
private:
    Orario& orario; // Riferimento non proprietario.
    MotoreScambi motore;
    ValidatoreOrario validatore;

    int LeggiIntero(std::string messaggio, int minimo, int massimo);
    Posizione LeggiPosizione();
    void VisualizzaOrario();
    void VisualizzaTessera();
    void RichiediScambio();
    void VerificaOrario();
    void MostraMenu();
public:
    InterfacciaConsole(Orario& orario);
    void Esegui();
};
