#pragma once
#include <string>
#include "Model/Orario.h"
#include "Model/ElencoClassi.h"
#include "Engine/MotoreScambi.h"
#include "Engine/ValidatoreOrario.h"

class InterfacciaConsole {
private:
    Orario& orario; // Riferimento non proprietario.
    ElencoClassi& elencoClassi; // Riferimento non proprietario.
    MotoreScambi motore;
    ValidatoreOrario validatore;

    int LeggiIntero(std::string messaggio, int minimo, int massimo);
    Posizione LeggiPosizione();
    void VisualizzaOrario();
    void VisualizzaTessera();
    void RichiediScambio();
    void VerificaOrario();
    void MostraMenu();
    void VisualizzaClassi();
    void CercaClasse();
    void AggiungiClasse();
public:
    InterfacciaConsole(Orario& orario, ElencoClassi& elencoClassi);
    void Esegui();
};
